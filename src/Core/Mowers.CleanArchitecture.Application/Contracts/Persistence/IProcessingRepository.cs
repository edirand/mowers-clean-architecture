using Mowers.CleanArchitecture.Domain.Entities;

namespace Mowers.CleanArchitecture.Application.Contracts.Persistence;

/// <summary>
/// Defines a repository for <see cref="FileProcessing"/> entities.
/// </summary>
public interface IProcessingRepository : IAsyncRepository<FileProcessing>
{
    
}
