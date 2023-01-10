using Microsoft.AspNetCore.Builder;
using Mower.CleanArchitecture.FileProcessorWorker;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();

var app = builder
        .Build()
        .ConfigureApplication()
    ;

app.Run();