namespace Mowers.CleanArchitecture.Application.Features.Mowers.Commands.UploadFile;

/// <summary>
/// A response for <see cref="UploadFileCommand"/> containing the ID of the scheduled processing.
/// </summary>
/// <param name="Id">The ID of the scheduled processing.</param>
/// <param name="CreatedAt">The creation date of the processing.</param>
public record UploadFileCommandResponse(Guid Id, DateTime CreatedAt);
