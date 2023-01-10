using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
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

        var mongoSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>()!;

        return services
                .AddSingleton<IMongoClient>(provider =>
                {
                    var settings = MongoClientSettings.FromConnectionString(mongoSettings.ConnectionString);
                    settings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
                    return new MongoClient(settings);
                })
                .AddSingleton<IMongoDatabase>(provider =>
                    provider.GetRequiredService<IMongoClient>().GetDatabase(mongoSettings.Database))
                .AddSingleton<MongoDbRepository>()
                .AddSingleton<IProcessingRepository, FileProcessingRepository>()
                .AddHealthChecks()
                .AddMongoDb(mongoSettings.ConnectionString, mongoSettings.Database, mongoSettings.Database, HealthStatus.Unhealthy, new []{"database", "MongoDb"})
                .Services
            ;
    }
}
