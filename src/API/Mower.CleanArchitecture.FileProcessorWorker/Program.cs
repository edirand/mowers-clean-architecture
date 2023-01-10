using Mower.CleanArchitecture.FileProcessorWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(StartupExtensions.ConfigureServices)
    .Build();

await host.RunAsync();
