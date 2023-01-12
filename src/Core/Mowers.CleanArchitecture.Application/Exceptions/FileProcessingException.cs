using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Mowers.CleanArchitecture.Application.Exceptions;

/// <summary>
/// Represents errors that occur during application execution when the processing of a file failed.
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class FileProcessingException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileProcessingException"/> class.
    /// </summary>
    /// <param name="name">The requested item's name.</param>
    /// <param name="key">The requested item's key.</param>
    /// <param name="exception">The inner exception.</param>
    public FileProcessingException(string name, object key, Exception exception)
        : base($"{name} ({key}) failed", exception)
    {
    }
    
    protected FileProcessingException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}