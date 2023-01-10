using Mowers.CleanArchitecture.Persistence.Attributes;

namespace Mowers.CleanArchitecture.Persistence.Models;

/// <summary>
/// A file processing mongo document.
/// </summary>
[MongoCollection("fileProcessing")]
public class FileProcessingDocument : MongoDocument
{
    /// <summary>
    /// The path of the file to process.
    /// </summary>
    public string FilePath { get; set; } = default!;
    
    /// <summary>
    /// Indicates if the processing is completed.
    /// </summary>
    public bool Completed { get; set; }

    /// <summary>
    /// The processing result containing mowers final position and orientation. 
    /// </summary>
    public IEnumerable<string> Mowers { get; set; } = default!;
}
