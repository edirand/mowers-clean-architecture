using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        var settings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();
        
        return services
                .AddSingleton<IMongoClient>(new MongoClient(settings!.ConnectionString))
                .AddSingleton<IMongoDatabase>(provider =>
                    provider.GetRequiredService<IMongoClient>().GetDatabase(settings.Database))
                .AddScoped<MongoDbRepository>()
                .AddScoped<IProcessingRepository, FileProcessingRepository>()
            ;
    }
}
