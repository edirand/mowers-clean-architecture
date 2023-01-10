using System.Reflection;
using MongoDB.Driver;
using Mowers.CleanArchitecture.Persistence.Attributes;
using Mowers.CleanArchitecture.Persistence.Models;

namespace Mowers.CleanArchitecture.Persistence;

/// <summary>
/// A MongoDb base repository.
/// </summary>
public class MongoDbRepository
{
    private readonly IMongoDatabase _database;

    /// <summary>
    /// Initializes a new instance of the <see cref="MongoDbRepository"/> class.
    /// </summary>
    /// <param name="database">The database.</param>
    public MongoDbRepository(IMongoDatabase database)
    {
        _database = database;
    }

    /// <summary>
    /// Gets the MongoDb collection to use for a given <see cref="MongoDocument"/> type.
    /// </summary>
    /// <param name="readPreference">The read preference to use to connect on the database.</param>
    /// <typeparam name="T">The <see cref="MongoDocument"/> type stored in the collection.</typeparam>
    /// <returns>An instance of <see cref="IMongoCollection{TDocument}"/>.</returns>
    public IMongoCollection<T> GetCollection<T>(ReadPreference? readPreference = null) where T : MongoDocument
    {
        return _database
            .WithReadPreference(readPreference ?? ReadPreference.Primary)
            .GetCollection<T>(GetCollectionName<T>())
            ;
    }

    private static string GetCollectionName<T>() where T : MongoDocument
    {
        return (typeof(T).GetCustomAttribute<MongoCollectionAttribute>())?.CollectionName!;
    }
}
