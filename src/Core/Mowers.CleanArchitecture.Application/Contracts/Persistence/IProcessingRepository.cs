using Mowers.CleanArchitecture.Domain.Entities;

namespace Mowers.CleanArchitecture.Application.Contracts.Persistence;

/// <summary>
/// Defines a repository for <see cref="FileProcessing"/> entities.
/// </summary>
public interface IProcessingRepository : IAsyncRepository<FileProcessing>
{
    /// <summary>
    /// Gets all the <see cref="FileProcessing"/> that has not been completed.
    /// </summary>
    /// <returns>The not completed <see cref="FileProcessing"/>.</returns>
    Task<IReadOnlyList<FileProcessing>> ListNotCompletedProcessing();
}
