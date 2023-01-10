using MediatR;
using Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessStoredFile;
using Mowers.CleanArchitecture.Application.Features.Mowers.Queries.GetNotCompletedFileProcessing;

namespace Mower.CleanArchitecture.FileProcessorWorker;

/// <summary>
/// A worker to run file processing asynchronously.
/// </summary>
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMediator _mediator;
    private const int RunDelayMs = 1000;

    /// <summary>
    /// Initializes a new instance of <see cref="Worker"/>.
    /// </summary>
    /// <param name="logger">An instance of <see cref="ILogger{Worker}"/>.</param>
    /// <param name="mediator">An instance of <see cref="IMediator"/>.</param>
    public Worker(ILogger<Worker> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
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
            if(notCompletedFileProcessing?.Ids == null) continue;

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
            _logger.LogError(e, "Loading of not completed files failed: {Exception}", e);
        }

        return null;
    }

    private async Task ProcessFile(Guid fileId, CancellationToken stoppingToken)
    {
        try
        {
            await _mediator.Send(new ProcessStoredFileCommand(fileId), stoppingToken);
            _logger.LogInformation("Processing of file {FileId} completed", fileId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Processing of file {FileId} failed: {Exception}", fileId, e);
        }
    }
}
