using System.Diagnostics;
using System.Diagnostics.Metrics;
using MediatR;
using Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessStoredFile;
using Mowers.CleanArchitecture.Application.Features.Mowers.Queries.GetNotCompletedFileProcessing;
using Mowers.CleanArchitecture.Infrastructure.Metrics;

namespace Mower.CleanArchitecture.FileProcessorWorker;

/// <summary>
/// A worker to run file processing asynchronously.
/// </summary>
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMediator _mediator;
    private const int RunDelayMs = 1000;
    private static readonly ActivitySource ActivitySource = new(nameof(Worker));
    private readonly Metrics _metrics;

    /// <summary>
    /// Initializes a new instance of <see cref="Worker"/>.
    /// </summary>
    /// <param name="logger">An instance of <see cref="ILogger{Worker}"/>.</param>
    /// <param name="mediator">An instance of <see cref="IMediator"/>.</param>
    public Worker(ILogger<Worker> logger, IMediator mediator, Metrics metrics)
    {
        _logger = logger;
        _mediator = mediator;
        _metrics = metrics;
    }

    /// <summary>
    /// Runs the processing of not completed stored files.
    /// </summary>
    /// <param name="stoppingToken">A cancellation token.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(RunDelayMs, stoppingToken);

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var notCompletedFileProcessing = await LoadNotCompletedFiles(stoppingToken);
            if (notCompletedFileProcessing?.Ids == null) continue;

            foreach (var fileId in notCompletedFileProcessing.Ids)
            {
                await ProcessFile(fileId, stoppingToken);
            }
        }
    }

    private async Task<NotCompletedFileProcessing?> LoadNotCompletedFiles(CancellationToken stoppingToken)
    {
        try
        {
            var filesToProcess = await _mediator.Send(new GetNotCompletedFileProcessingQuery(), stoppingToken);
            return filesToProcess;
        }
        catch (Exception e)
        {
            _logger.LogError("Loading of not completed files failed: {Exception}", e);
        }

        return null;
    }

    private async Task ProcessFile(Guid fileId, CancellationToken stoppingToken)
    {
        var sw = Stopwatch.StartNew();
        var activityName = $"{fileId} processing";
        using var activity = ActivitySource.StartActivity(activityName);
        activity?.AddTag("worker.processing.file_id", fileId);
        _metrics.AddProcessing();
        try
        {
            await _mediator.Send(new ProcessStoredFileCommand(fileId), stoppingToken);
            _logger.LogInformation("Processing of file {FileId} completed", fileId);
            activity?.AddTag("worker.processing.success", true);
            activity?.SetStatus(ActivityStatusCode.Ok);
            _metrics.RecordProcessingDuration(sw.ElapsedMilliseconds, true);
        }
        catch (Exception e)
        {
            _logger.LogError("Processing of file {FileId} failed: {Exception}", fileId, e);
            activity?.AddTag("worker.processing.success", false);
            activity?.SetStatus(ActivityStatusCode.Error);
            _metrics.AddProcessingError();
            _metrics.RecordProcessingDuration(sw.ElapsedMilliseconds, false);
        }
    }
}
