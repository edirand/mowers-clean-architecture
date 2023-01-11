using System.Reflection;
using HealthChecks.UI.Client;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Mowers.CleanArchitecture.Api.Extensions;
using Mowers.CleanArchitecture.Api.Extensions.Swagger;
using Mowers.CleanArchitecture.Application;
using Mowers.CleanArchitecture.Infrastructure;
using Mowers.CleanArchitecture.Infrastructure.Traces;
using Mowers.CleanArchitecture.Persistence;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Mowers.CleanArchitecture.Api;

/// <summary>
/// Extensions to configure startup.
/// </summary>
public static class StartupExtensions
{
    /// <summary>
    /// Configures services.
    /// </summary>
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddApplicationServices()
            .AddPersistenceServices(builder.Configuration)
            .AddInfrastructureServices()
            .AddControllers()
            .Services
            .ConfigureOpenTelemetry(builder.Configuration)
            .AddRouting(c => { c.LowercaseUrls = true; })
            .AddCors(options =>
            {
                options.AddPolicy("Open", configurePolicy => configurePolicy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                );
            })
            .AddVersioning()
            .AddSwagger()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            .ConfigureProblemDetails()
            .ConfigureHealthChecks()
            ;


        return builder;
    }

    private static IServiceCollection ConfigureHealthChecks(this IServiceCollection services)
    {
        return services
                .AddHealthChecks()
                .AddCheck("API", () => HealthCheckResult.Healthy(), new[] { "API" })
                .Services
            ;
    }

    private static IServiceCollection AddVersioning(this IServiceCollection services)
    {
        return services
                .AddApiVersioning(opt =>
                {
                    opt.DefaultApiVersion = new ApiVersion(1, 0);
                    opt.AssumeDefaultVersionWhenUnspecified = true;
                    opt.ReportApiVersions = true;
                    opt.ApiVersionReader = ApiVersionReader.Combine(
                            new UrlSegmentApiVersionReader(),
                            new HeaderApiVersionReader("x-api-version")
                        )
                        ;
                })
                .AddVersionedApiExplorer(setup =>
                {
                    setup.GroupNameFormat = "'v'VVV";
                    setup.SubstituteApiVersionInUrl = true;
                })
                .AddEndpointsApiExplorer()
            ;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        return services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                    typeof(StartupExtensions).Assembly.GetName().Name + ".xml"));
            })
            .ConfigureOptions<ConfigureSwaggerOptions>();
    }

    private static IServiceCollection ConfigureOpenTelemetry(this IServiceCollection services,
        IConfiguration configuration)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
            Assembly.GetExecutingAssembly().GetName().Name!,
            serviceVersion: Assembly.GetExecutingAssembly().GetName().Version!.ToString());
        var jaegerConfig = configuration.GetSection("Observability:Jaeger").Get<JaegerConfiguration>();

        return services
                .AddOpenTelemetry()
                .WithTracing(builder =>
                    builder
                        .AddConsoleExporter()
                        .SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation(i =>
                            i.Filter = context => !string.IsNullOrWhiteSpace(context.Request.Path.Value)
                                                  && !context.Request.Path.Value.Contains("health",
                                                      StringComparison.InvariantCultureIgnoreCase)
                        )
                        .AddMongoDBInstrumentation()
                        .AddJaegerExporter(options =>
                        {
                            options.AgentHost = jaegerConfig!.Host;
                            options.AgentPort = jaegerConfig.Port;
                        })
                        .AddSource("Api")
                )
                .WithMetrics(builder =>
                    builder
                        .SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation(i =>
                            i.Filter = (_, context) => !string.IsNullOrWhiteSpace(context.Request.Path.Value)
                                                       && !context.Request.Path.Value.Contains("health",
                                                           StringComparison.InvariantCultureIgnoreCase)
                        )
                        .AddPrometheusExporter()
                )
                .StartWithHost()
                .Services
            ;
    }

    /// <summary>
    /// Configures the application.
    /// </summary>
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app
            .UseHttpsRedirection()
            .UseProblemDetails()
            .UseRouting()
            .UseOpenTelemetryPrometheusScrapingEndpoint()
            .UseCors("Open")
            .UseSwagger()
            .UseSwaggerUI(c =>
            {
                foreach (var groupName in apiVersionDescriptionProvider.ApiVersionDescriptions.Reverse()
                             .Select(x => x.GroupName))
                {
                    c.SwaggerEndpoint($"/swagger/{groupName}/swagger.json",
                        groupName.ToUpperInvariant());
                }
            })
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