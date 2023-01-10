using AutoMapper;
using MongoDB.Driver;
using Mowers.CleanArchitecture.Application.Contracts.Persistence;
using Mowers.CleanArchitecture.Domain.Entities;
using Mowers.CleanArchitecture.Persistence.Models;

namespace Mowers.CleanArchitecture.Persistence.Repositories;

/// <summary>
/// An implementation of <see cref="IProcessingRepository"/> using MongoDb.
/// </summary>
public class FileProcessingRepository : BaseRepository<FileProcessing, FileProcessingDocument>, IProcessingRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileProcessingRepository"/> class.
    /// </summary>
    /// <param name="repository">An instance of <see cref="MongoDbRepository"/> to use to access database.</param>
    /// <param name="mapper">An instance of <see cref="IMapper"/> to map <see cref="FileProcessing"/> to <see cref="FileProcessingDocument"/>.</param>
    public FileProcessingRepository(MongoDbRepository repository, IMapper mapper) : base(repository, mapper)
    {
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<FileProcessing>> ListNotCompletedProcessing()
    {
        var filter = Builders<FileProcessingDocument>.Filter.Eq(x => x.Completed, false);
        var items = await MongoDbRepository.GetCollection<FileProcessingDocument>().FindAsync(filter);
        return Mapper.Map<IReadOnlyList<FileProcessing>>(await items.ToListAsync());
    }
}
