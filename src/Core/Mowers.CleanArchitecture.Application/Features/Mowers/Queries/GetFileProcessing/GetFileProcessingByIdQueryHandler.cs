using MediatR;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;
using Mowers.CleanArchitecture.Application.Exceptions;
using Mowers.CleanArchitecture.Domain.Entities;

namespace Mowers.CleanArchitecture.Application.Features.Mowers.Queries.GetFileProcessing;

/// <summary>
/// A handler for <see cref="GetFileProcessingByIdQuery"/>.
/// </summary>
public class GetFileProcessingByIdQueryHandler : IRequestHandler<GetFileProcessingByIdQuery, FileProcessingResult>
{
    private readonly IProcessingRepository _processingRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetFileProcessingByIdQueryHandler"/> class.
    /// </summary>
    /// <param name="processingRepository">An instance of <see cref="IProcessingRepository"/>.</param>
    public GetFileProcessingByIdQueryHandler(IProcessingRepository processingRepository)
    {
        _processingRepository = processingRepository;
    }
    
    /// <summary>
    /// Gets a file processing result by ID.
    /// </summary>
    /// <param name="request">The request handled.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The file processing result.</returns>
    /// <exception cref="NotFoundException">When the requested item does not exist.</exception>
    public async Task<FileProcessingResult> Handle(GetFileProcessingByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _processingRepository.GetById(request.Id);
        if (item == null) throw new NotFoundException(nameof(FileProcessing), request.Id);

        return new FileProcessingResult(item.Id, item.Completed, item.Mowers, item.CreatedAt, item.UpdatedAt);
    }
}
