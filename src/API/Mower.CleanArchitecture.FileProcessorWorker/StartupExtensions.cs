using System.Reflection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Mowers.CleanArchitecture.Application;
using Mowers.CleanArchitecture.Infrastructure;
using Mowers.CleanArchitecture.Infrastructure.Metrics;
using Mowers.CleanArchitecture.Infrastructure.Traces;
using Mowers.CleanArchitecture.Persistence;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Mower.CleanArchitecture.FileProcessorWorker;

/// <summary>
/// Extensions to configure the application dependencies.
/// </summary>
public static class StartupExtensions
{
    /// <summary>
    /// Configures the application dependencies.
    /// </summary>
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddApplicationServices()
            .AddPersistenceServices(builder.Configuration)
            .AddInfrastructureServices()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            .ConfigureOpenTelemetry(builder.Configuration)
            .AddHostedService<Worker>()
            .AddControllers()
            .Services
            .AddRouting()
            .AddHealthChecks()
            .AddCheck("Worker", () => HealthCheckResult.Healthy(), new []{"Worker"})
            ;
    }

    private static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services,
        IConfiguration configuration)
    {
        var meter = new Metrics(nameof(Worker), nameof(Worker).ToLowerInvariant());
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
            Assembly.GetExecutingAssembly().GetName().Name!,
            serviceVersion: Assembly.GetExecutingAssembly().GetName().Version!.ToString());
        var jaegerConfig = configuration.GetSection("Observability:Jaeger").Get<JaegerConfiguration>();

        services
            .AddSingleton(meter)
            .AddOpenTelemetry()
            .WithTracing(builder =>
                builder
                    .SetResourceBuilder(resourceBuilder)
                    .AddMongoDBInstrumentation()
                    .AddConsoleExporter()
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = jaegerConfig!.Host;
                        options.AgentPort = jaegerConfig.Port;
                    })
                    .AddSource(meter.MeterName)
            )
            .WithMetrics(builder =>
                builder
                    .SetResourceBuilder(resourceBuilder)
                    .AddMeter(meter.MeterName)
                    .AddAspNetCoreInstrumentation(i =>
                        i.Filter = (_, context) => !string.IsNullOrWhiteSpace(context.Request.Path.Value)
                                                   && !context.Request.Path.Value.Contains("health",
                                                       StringComparison.InvariantCultureIgnoreCase)
                    )
                    .AddPrometheusExporter()
            )
            .StartWithHost()
            ;

        return services;
    }

    /// <summary>
    /// Configures the application.
    /// </summary>
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app
            .UseHttpsRedirection()
            .UseRouting()
            .UseOpenTelemetryPrometheusScrapingEndpoint()
            ;

        
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.MapControllers();

        return app;
    }
}