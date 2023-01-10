using Mowers.CleanArchitecture.Domain.Common;

namespace Mowers.CleanArchitecture.Application.Contracts.Persistence;

/// <summary>
/// Defines a generic asynchronous repository.
/// </summary>
/// <typeparam name="T">The type of the <see cref="IEntity"/> stored in the repository.</typeparam>
public interface IAsyncRepository<T> where T: IEntity
{
    /// <summary>
    /// Gets an entity using it's ID.
    /// </summary>
    /// <param name="id">The ID of the entity to find.</param>
    /// <returns>An instance of the entity if it exists; otherwise, null.</returns>
    Task<T?> GetById(Guid id);
    
    /// <summary>
    /// Gets all the entities stored in the repository.
    /// </summary>
    /// <returns>An instance of <see cref="IReadOnlyList{T}"/> containing the stored entities.</returns>
    Task<IReadOnlyList<T>> ListAll();
    
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    Task Add(T entity);
    
    /// <summary>
    /// Updates an entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    Task Update(T entity);
    
    /// <summary>
    /// Deletes an entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    Task Delete(T entity);
}
