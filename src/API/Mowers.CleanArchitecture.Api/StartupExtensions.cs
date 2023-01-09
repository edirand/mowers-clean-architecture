using Microsoft.OpenApi.Models;
using Mowers.CleanArchitecture.Application;

namespace Mowers.CleanArchitecture.Api;

public static class StartupExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddApplicationServices()
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
            .AddSwagger()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            ;


        return builder;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        return services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Mowers Clean Architecture API",
            });
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, typeof(StartupExtensions).Assembly.GetName().Name + ".xml"));
        });
    }

    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        app
            .UseHttpsRedirection()
            .UseRouting()
            .UseCors("Open")
            .UseSwagger()
            .UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mowers Clean Architecture API"); })
            ;

        app.MapControllers();

        return app;
    }
}