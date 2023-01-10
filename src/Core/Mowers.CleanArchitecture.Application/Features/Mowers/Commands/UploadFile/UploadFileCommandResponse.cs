namespace Mowers.CleanArchitecture.Application.Features.Mowers.Commands.UploadFile;

/// <summary>
/// A response for <see cref="UploadFileCommand"/> containing the ID of the scheduled processing.
/// </summary>
/// <param name="ProcessingId">The ID of the scheduled processing.</param>
public record UploadFileCommandResponse(Guid ProcessingId);
