using MediatR;

namespace Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessStoredFile;

/// <summary>
/// A command to run the processing of a stored file.
/// </summary>
/// <param name="Id">The identifier of the file to process.</param>
public record ProcessStoredFileCommand(Guid Id) : IRequest;
