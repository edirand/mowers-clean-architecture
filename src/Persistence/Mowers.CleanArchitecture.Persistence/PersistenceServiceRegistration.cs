using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;
using Mowers.CleanArchitecture.Persistence.Repositories;

namespace Mowers.CleanArchitecture.Persistence;

/// <summary>
/// Extension class to inject necessary services in DI container.
/// </summary>
[ExcludeFromCodeCoverage]
public static class PersistenceServiceRegistration
{
    /// <summary>
    /// Injects necessary services in DI container.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The configured services collection.</returns>
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var conventions = new ConventionPack
        {
            new CamelCaseElementNameConvention(),
            new IgnoreIfNullConvention(true)
        };

        ConventionRegistry.Register("Custom conventions", conventions, x => true);
        
        return services
                .Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"))
                .AddSingleton<IMongoClient>(provider => new MongoClient(provider.GetRequiredService<IOptions<MongoDbSettings>>().Value.ConnectionString))
                .AddSingleton<IMongoDatabase>(provider =>
                    provider.GetRequiredService<IMongoClient>().GetDatabase(provider.GetRequiredService<IOptions<MongoDbSettings>>().Value.Database))
                .AddSingleton<MongoDbRepository>()
                .AddSingleton<IProcessingRepository, FileProcessingRepository>()
            ;
    }
}
