using MediatR;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;

namespace Mowers.CleanArchitecture.Application.Features.Mowers.Queries.GetNotCompletedFileProcessing;

/// <summary>
/// A handler for <see cref="GetNotCompletedFileProcessingQuery"/>.
/// </summary>
public class GetNotCompletedFileProcessingQueryHandler : IRequestHandler<GetNotCompletedFileProcessingQuery, NotCompletedFileProcessing>
{
    private readonly IProcessingRepository _processingRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="GetNotCompletedFileProcessingQueryHandler"/>.
    /// </summary>
    /// <param name="processingRepository">An instance of <see cref="IProcessingRepository"/>.</param>
    public GetNotCompletedFileProcessingQueryHandler(IProcessingRepository processingRepository)
    {
        _processingRepository = processingRepository;
    }
    
    /// <summary>
    /// Gets all the file processing that has not been completed.
    /// </summary>
    /// <param name="request">An instance of <see cref="GetNotCompletedFileProcessingQuery"/>.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>An instance of the <see cref="NotCompletedFileProcessing"/> class containing not completed processing.</returns>
    public async Task<NotCompletedFileProcessing> Handle(GetNotCompletedFileProcessingQuery request, CancellationToken cancellationToken)
    {
        var notCompletedProcessing = await _processingRepository.ListNotCompletedProcessing();
        return new NotCompletedFileProcessing(notCompletedProcessing.Select(x => x.Id));
    }
}
