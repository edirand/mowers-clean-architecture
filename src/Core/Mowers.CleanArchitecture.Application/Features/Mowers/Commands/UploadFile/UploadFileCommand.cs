using MediatR;

namespace Mowers.CleanArchitecture.Application.Features.Mowers.Commands.UploadFile;

/// <summary>
/// A command to upload a file to process.
/// </summary>
/// <param name="Data">The content of the file</param>
public record UploadFileCommand(Stream Data) : IRequest<UploadFileCommandResponse>;
