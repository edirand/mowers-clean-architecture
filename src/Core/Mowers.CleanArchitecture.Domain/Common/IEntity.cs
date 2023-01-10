namespace Mowers.CleanArchitecture.Domain.Common;

/// <summary>
/// Defines an entity.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// The identifier of the entity.
    /// </summary>
    public Guid Id { get; }
    
    /// <summary>
    /// The creation date and time of the entity.
    /// </summary>
    public DateTime CreatedAt { get; }
    
    /// <summary>
    /// The last update date and time of the entity.
    /// </summary>
    public DateTime UpdatedAt { get; }
}
