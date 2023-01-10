namespace Mowers.CleanArchitecture.Persistence;

/// <summary>
/// Settings for MongoDB
/// </summary>
public class MongoDbSettings
{
    /// <summary>
    /// The connection string.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
    
    /// <summary>
    /// The name of the database.
    /// </summary>
    public string Database { get; set; } = string.Empty;
}
