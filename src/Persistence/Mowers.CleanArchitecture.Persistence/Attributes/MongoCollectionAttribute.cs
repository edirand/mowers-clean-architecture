namespace Mowers.CleanArchitecture.Persistence.Attributes;

/// <summary>
/// An attribute to indicate the name of the collection where the decorated class instances are stored.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class MongoCollectionAttribute : Attribute
{
    /// <summary>
    /// The name of the collection.
    /// </summary>
    public string CollectionName { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="MongoCollectionAttribute"/> class.
    /// </summary>
    /// <param name="collectionName">The name of the collection.</param>
    public MongoCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}
