namespace Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessFile;

/// <summary>
/// A response for <see cref="ProcessFileCommand"/> containing the final position and orientation of the mowers.
/// </summary>
/// <param name="Mowers">The final position and orientation of the mowers as string (X Y O).</param>
public record ProcessFileCommandResponse(IEnumerable<string> Mowers);
