using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Mowers.CleanArchitecture.Application.Contracts.Infrastructure.FileStorage;
using Mowers.CleanArchitecture.Infrastructure.FileStorage;

namespace Mowers.CleanArchitecture.Infrastructure;

/// <summary>
/// Extension class to inject necessary services in DI container.
/// </summary>
[ExcludeFromCodeCoverage]
public static class InfrastructureServiceRegistration
{
    
    /// <summary>
    /// Injects necessary services in DI container.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <returns>The configured services collection.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        return services
                .AddSingleton<IFileStorage, LocalFileSystem>()
            ;
    }
}
