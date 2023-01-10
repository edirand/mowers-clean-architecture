using Mowers.CleanArchitecture.Domain.Common;

namespace Mowers.CleanArchitecture.Domain.Entities;

/// <summary>
/// Represent a file processing.
/// </summary>
public class FileProcessing : IEntity
{
    /// <summary>
    /// The identifier of the file processing.
    /// </summary>
    public Guid Id { get; set; }

    /// <inheritdoc />
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc />
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// Indicates if the processing has been completed.
    /// </summary>
    public bool Completed{get; set; }
    
    /// <summary>
    /// The file path.
    /// </summary>
    public string FilePath { get; set; }
    
    /// <summary>
    /// The processing result containing mowers final position and orientation. 
    /// </summary>
    public IEnumerable<string> Mowers{get; set; }
}
