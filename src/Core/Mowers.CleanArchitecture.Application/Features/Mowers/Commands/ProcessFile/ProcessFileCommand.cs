using MediatR;

namespace Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessFile;

/// <summary>
/// A command to run file processing.
/// </summary>
/// <param name="Data">The content of the file</param>
public record ProcessFileCommand(Stream Data): IRequest<ProcessFileCommandResponse>;
