namespace Mowers.CleanArchitecture.Application.Contracts.Infrastructure.FileStorage;

/// <summary>
/// Defines a file storage to store files to process.
/// </summary>
public interface IFileStorage
{
    /// <summary>
    /// Stores a file.
    /// </summary>
    /// <param name="data">The data of the file.</param>
    /// <returns>The path of the stored file.</returns>
    public Task<string> Store(Stream data);

    /// <summary>
    /// Loads a file.
    /// </summary>
    /// <param name="filePath">The path of the stored file.</param>
    /// <returns>The data of the file.</returns>
    Stream Load(string filePath);
}
