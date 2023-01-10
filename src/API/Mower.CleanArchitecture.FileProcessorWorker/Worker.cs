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
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var filesToProcess = await _mediator.Send(new GetNotCompletedFileProcessingQuery(), stoppingToken);
            
            foreach (var fileId in filesToProcess.Ids)
            {
                await _mediator.Send(new ProcessStoredFileCommand(fileId), stoppingToken);
            }
        }
    }
}
