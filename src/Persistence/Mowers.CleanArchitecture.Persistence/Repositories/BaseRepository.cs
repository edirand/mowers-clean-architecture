using AutoMapper;
using MongoDB.Driver;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;
using Mowers.CleanArchitecture.Domain.Common;
using Mowers.CleanArchitecture.Persistence.Models;

namespace Mowers.CleanArchitecture.Persistence.Repositories;

/// <summary>
/// A base implementation of <see cref="IAsyncRepository{TEntity}"/>
/// </summary>
/// <typeparam name="TEntity">The entity type managed by the repository.</typeparam>
/// <typeparam name="TDocument">The <see cref="MongoDocument"/> type stored in the collection.</typeparam>
public class BaseRepository<TEntity, TDocument> : IAsyncRepository<TEntity>
    where TEntity : IEntity where TDocument : MongoDocument
{
    protected readonly MongoDbRepository MongoDbRepository;
    protected readonly IMapper Mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseRepository{TEntity,TDocument}"/> class.
    /// </summary>
    /// <param name="repository">The MongoDb repository to use.</param>
    /// <param name="mapper">An instance of <see cref="IMapper"/> to map <see cref="TEntity"/> to <see cref="TDocument"/>.</param>
    protected BaseRepository(MongoDbRepository repository, IMapper mapper)
    {
        MongoDbRepository = repository;
        Mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<TEntity?> GetById(Guid id)
    {
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, id);
        var t = await (
            await MongoDbRepository
                .GetCollection<TDocument>()
                .FindAsync(filter)
        ).FirstOrDefaultAsync();

        return t == null ? default : Mapper.Map<TEntity>(t);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TEntity>> ListAll()
    {
        var documents = await (
            await MongoDbRepository
                .GetCollection<TDocument>()
                .FindAsync(FilterDefinition<TDocument>.Empty)
        ).ToListAsync();

        return Mapper.Map<IReadOnlyList<TEntity>>(documents);
    }

    /// <inheritdoc />
    public async Task Add(TEntity entity)
    {
        var document = Mapper.Map<TDocument>(entity);
        await MongoDbRepository.GetCollection<TDocument>().InsertOneAsync(document);
    }

    /// <inheritdoc />
    public async Task Update(TEntity entity)
    {
        var document = Mapper.Map<TDocument>(entity);
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, document.Id);
        await MongoDbRepository.GetCollection<TDocument>().ReplaceOneAsync(filter, document);
    }

    /// <inheritdoc />
    public async Task Delete(TEntity entity)
    {
        var filter = Builders<TDocument>.Filter.Eq(x => x.Id, entity.Id);
        await MongoDbRepository.GetCollection<TDocument>().DeleteOneAsync(filter);
    }
}
