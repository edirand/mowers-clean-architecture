using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Mowers.CleanArchitecture.Application.Features.Mowers.Commands.ProcessFile;
using Shouldly;
using Xunit;

namespace Mowers.CleanArchitecture.Application.UnitTests.Features.Mowers.Commands.ProcessFile;

/// <summary>
/// Tests for <see cref="ProcessFileCommandHandler"/> class.
/// </summary>
public class ProcessFileCommandHandlerTests
{
    [Fact]
    public async Task ShouldProcessFile()
    {
        var handler = new ProcessFileCommandHandler();
        using var stream = new MemoryStream();
        await using var streamWriter = new StreamWriter(stream);
        await streamWriter.WriteLineAsync("5 5");
        await streamWriter.WriteLineAsync("1 2 N");
        await streamWriter.WriteLineAsync("LFLFLFLFF");
        await streamWriter.WriteLineAsync("3 3 E");
        await streamWriter.WriteLineAsync("FFRFFRFRRF");
        await streamWriter.FlushAsync();
        stream.Seek(0, SeekOrigin.Begin);

        var result = await handler.Handle(new ProcessFileCommand(stream), CancellationToken.None);
        
        result.Mowers.ShouldBe(new List<string>
        {
            "1 3 N",
            "5 1 E"
        });
    }

     [Fact]
    public async Task ShouldProcessFileWithMowerOutsideOfLawn()
    {
        var handler = new ProcessFileCommandHandler();
        using var stream = new MemoryStream();
        await using var streamWriter = new StreamWriter(stream);
        await streamWriter.WriteLineAsync("5 5");
        await streamWriter.WriteLineAsync("0 0 S");
        await streamWriter.WriteLineAsync("F");
        await streamWriter.FlushAsync();
        stream.Seek(0, SeekOrigin.Begin);

        var result = await handler.Handle(new ProcessFileCommand(stream), CancellationToken.None);
        
        result.Mowers.ShouldBe(new List<string>
        {
            "0 0 S"
        });
    }

    [Fact]
    public async Task ShouldProcessFileWithMowersCollision()
    {
        var handler = new ProcessFileCommandHandler();
        using var stream = new MemoryStream();
        await using var streamWriter = new StreamWriter(stream);
        await streamWriter.WriteLineAsync("5 5");
        await streamWriter.WriteLineAsync("0 0 N");
        await streamWriter.WriteLineAsync("L");
        await streamWriter.WriteLineAsync("0 1 S");
        await streamWriter.WriteLineAsync("F");
        await streamWriter.FlushAsync();
        stream.Seek(0, SeekOrigin.Begin);

        var result = await handler.Handle(new ProcessFileCommand(stream), CancellationToken.None);
        
        result.Mowers.ShouldBe(new List<string>
        {
            "0 0 W",
            "0 1 S",
        });
    }
}