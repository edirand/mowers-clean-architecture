// See https://aka.ms/new-console-template for more information

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Mowers.CleanArchitecture.Application;
using Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessFile;

var serviceProvider = new ServiceCollection()
        .AddApplicationServices()
        .BuildServiceProvider()
    ;

var mediator = serviceProvider.GetRequiredService<IMediator>();

var filePath = args[0];
var file = File.OpenRead(filePath);

var result = await mediator.Send(new ProcessFileCommand(file));

foreach (var mower in result.Mowers)
{
    Console.WriteLine(mower);
}

Console.ReadLine();