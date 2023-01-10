using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mowers.CleanArchitecture.Persistence.Models;

/// <summary>
/// A base MongoDb document.
/// </summary>
public class MongoDocument
{
    /// <summary>
    /// The identifier of the document.
    /// </summary>
    [BsonId, BsonRepresentation(BsonType.String)]
    public string Id { get; set; }
    
    /// <summary>
    /// The date and time of the creation of the document.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// The date and time of the last update of the document.
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
