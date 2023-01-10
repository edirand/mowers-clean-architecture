using MediatR;
using Mowers.CleanArchitecture.Application.Contracts.Infrastructure.FileStorage;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;
using Mowers.CleanArchitecture.Application.Exceptions;
using Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessFile;
using Mowers.CleanArchitecture.Domain.Entities;

namespace Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessStoredFile;

/// <summary>
/// A handler for <see cref="ProcessStoredFileCommand"/>.
/// </summary>
public class ProcessStoredFileCommandHandler : IRequestHandler<ProcessStoredFileCommand>
{
    private readonly IFileStorage _fileStorage;
    private readonly IProcessingRepository _processingRepository;
    private readonly IRequestHandler<ProcessFileCommand, ProcessFileCommandResponse> _handler;

    /// <summary>
    /// Initializes a new instance of <see cref="ProcessStoredFileCommandHandler"/>.
    /// </summary>
    /// <param name="fileStorage">An instance of <see cref="IFileStorage"/>.</param>
    /// <param name="processingRepository">An instance of <see cref="IProcessingRepository"/>.</param>
    /// <param name="handler">An instance of <see cref="IRequestHandler{ProcessFileCommand, ProcessFileCommandResponse}"/>.</param>
    public ProcessStoredFileCommandHandler(IFileStorage fileStorage, IProcessingRepository processingRepository, IRequestHandler<ProcessFileCommand, ProcessFileCommandResponse> handler)
    {
        _fileStorage = fileStorage;
        _processingRepository = processingRepository;
        _handler = handler;
    }
    
    /// <summary>
    /// Runs the processing of a stored file and saves the result.
    /// </summary>
    /// <remarks>This handler internally uses an instance of <see cref="IRequestHandler{ProcessFileCommand, ProcessFileCommandResponse}"/> to run the processing of the file content.</remarks>
    /// <param name="request">An instance of <see cref="ProcessStoredFileCommand"/> containing the ID of the file to process.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="NotFoundException">When the requested file does not exist.</exception>
    public async Task<Unit> Handle(ProcessStoredFileCommand request, CancellationToken cancellationToken)
    {
        var processing = await _processingRepository.GetById(request.Id);
        if (processing == null) throw new NotFoundException(nameof(FileProcessing), request.Id);

        var data = _fileStorage.Load(processing.FilePath);
        var result = await _handler.Handle(new ProcessFileCommand(data), cancellationToken);
        processing.Completed = true;
        processing.UpdatedAt = DateTime.Now;
        processing.Mowers = result.Mowers;
        await _processingRepository.Update(processing);
        
        return Unit.Value;
    }
}