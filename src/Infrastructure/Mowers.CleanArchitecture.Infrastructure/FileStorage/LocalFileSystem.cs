using Mowers.CleanArchitecture.Application.Contracts.Infrastructure.FileStorage;

namespace Mowers.CleanArchitecture.Infrastructure.FileStorage;

/// <summary>
/// An implementation of <see cref="IFileStorage"/> using local file system.
/// </summary>
public class LocalFileSystem : IFileStorage
{
    private const string Directory = "storage";

    /// <summary>
    /// Initializes a new instance of <see cref="LocalFileSystem"/> class.
    /// </summary>
    public LocalFileSystem()
    {
        var info = new DirectoryInfo(Path.Combine(Path.GetTempPath(),Directory));  
        if (!info.Exists) {  
            info.Create();  
        }
    }

    /// <inheritdoc />
    public Task<string> Store(Stream data)
    {
        var fileName = Path.GetRandomFileName();
        var filePath = Path.Combine(Path.GetTempPath(), Directory, fileName);
        using var outputFileStream = new FileStream(filePath, FileMode.Create);
        data.CopyTo(outputFileStream);

        return Task.FromResult(fileName);
    }

    /// <inheritdoc />
    public Stream Load(string filePath)
    {
        var fileFullPath = Path.Combine(Path.GetTempPath(), Directory, filePath);
        return File.OpenRead(fileFullPath);
    }
}
