using Mowers.CleanArchitecture.Api;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();

var app = builder
    .Build()
    .ConfigureApplication()
    ;

app.Run();

public partial class Program { }
