namespace Mowers.CleanArchitecture.Api.Models;

/// <summary>
/// A result for file processing.
/// </summary>
public class FileProcessingResult
{
    /// <summary>
    /// The final position and orientation of the mowers.
    /// </summary>
    /// <example>["1 2 N", "3 3 S"]</example>
    public IEnumerable<string> Mowers { get; set; }
}