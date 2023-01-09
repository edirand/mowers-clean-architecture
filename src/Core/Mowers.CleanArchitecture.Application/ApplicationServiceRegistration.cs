using System.Diagnostics.CodeAnalysis;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Mowers.CleanArchitecture.Application;

/// <summary>
/// Extension class to inject necessary services in DI container.
/// </summary>
[ExcludeFromCodeCoverage]
public static class ApplicationServiceRegistration
{
    /// <summary>
    /// Injects necessary services in DI container.
    /// </summary>
    /// <param name="services">The services collection.</param>
    /// <returns>The configured services collection.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddMediatR(AppDomain.CurrentDomain.GetAssemblies())
            ;
    }
}