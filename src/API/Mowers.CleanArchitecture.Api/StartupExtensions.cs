using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Mowers.CleanArchitecture.Api.Swagger;
using Mowers.CleanArchitecture.Application;
using Mowers.CleanArchitecture.Infrastructure;
using Mowers.CleanArchitecture.Persistence;

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
            ;


        return builder;
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

    /// <summary>
    /// Configures the application.
    /// </summary>
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        app
            .UseHttpsRedirection()
            .UseRouting()
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

        app.MapControllers();

        return app;
    }
}