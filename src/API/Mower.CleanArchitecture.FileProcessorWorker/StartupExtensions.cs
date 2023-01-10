using Mowers.CleanArchitecture.Application;
using Mowers.CleanArchitecture.Infrastructure;
using Mowers.CleanArchitecture.Persistence;

namespace Mower.CleanArchitecture.FileProcessorWorker;

/// <summary>
/// Extensions to configure the application dependencies.
/// </summary>
public static class StartupExtensions
{
    /// <summary>
    /// Configures the application dependencies.
    /// </summary>
    public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        var configuration = context.Configuration;
        services
            .AddApplicationServices()
            .AddPersistenceServices(configuration)
            .AddInfrastructureServices()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            ;
        
        services.AddHostedService<Worker>();
    }
}
