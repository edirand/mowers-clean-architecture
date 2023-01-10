using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mowers.CleanArchitecture.Api.IntegrationTests.Mocks;
using Mowers.CleanArchitecture.Application.Contracts.Infrastructure.FileStorage;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;

namespace Mowers.CleanArchitecture.Api.IntegrationTests.Base;

public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .UseContentRoot(".")
            .UseConfiguration(new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .Build())
            .UseEnvironment("tests")
            .ConfigureTestServices(services =>
            {
                services.RemoveAll<IProcessingRepository>();
                services.AddScoped<IProcessingRepository>(_=>RepositoryMocks.GetProcessingRepository().Object);
                services.AddScoped<IFileStorage>(_=>RepositoryMocks.GetFileStorage().Object);
            });

        base.ConfigureWebHost(builder);
    }
}