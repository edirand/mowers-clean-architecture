using MediatR;
using Mowers.CleanArchitecture.Application.Contracts.Infrastructure.FileStorage;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;
using Mowers.CleanArchitecture.Domain.Entities;

namespace Mowers.CleanArchitecture.Application.Features.Mowers.Commands.UploadFile;

/// <summary>
/// A handler for <see cref="UploadFileCommand"/>.
/// </summary>
public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, UploadFileCommandResponse>
{
    private readonly IFileStorage _fileStorage;
    private readonly IProcessingRepository _processingRepository;

    /// <summary>
    /// Initializes a new instance of <see cref="UploadFileCommandHandler"/> class.
    /// </summary>
    /// <param name="fileStorage">A file storage.</param>
    /// <param name="processingRepository">A repository for file processing.</param>
    public UploadFileCommandHandler(IFileStorage fileStorage, IProcessingRepository processingRepository)
    {
        _fileStorage = fileStorage;
        _processingRepository = processingRepository;
    }
    
    /// <summary>
    /// Uploads a file and schedule it to be processed later.
    /// </summary>
    /// <param name="request">The request containing the file data to process.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A instance of <see cref="UploadFileCommandResponse"/> class containing the identifier of the processing scheduled.</returns>
    public async Task<UploadFileCommandResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var path = await _fileStorage.Store(request.Data);
        var fileProcessing = new FileProcessing{Id = Guid.NewGuid(), FilePath = path, CreatedAt = DateTime.Now};
        await _processingRepository.Add(fileProcessing);
        return new UploadFileCommandResponse(fileProcessing.Id);
    }
}
