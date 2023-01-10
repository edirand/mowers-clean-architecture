using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Mowers.CleanArchitecture.Application.Exceptions;

/// <summary>
/// Represents errors that occur during application execution when a requested item does not exist.
/// </summary>
[Serializable]
[ExcludeFromCodeCoverage]
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="name">The requested item's name.</param>
    /// <param name="key">The requested item's key.</param>
    public NotFoundException(string name, object key)
        : base($"{name} ({key}) is not found")
    {
    }
    
    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}