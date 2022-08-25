using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace CloudStorage.Infrastructure.Data;

public class MongoRepository<TEntity> : IMongoRepository<TEntity>
    where TEntity : IEntity
{
    private readonly IMongoCollection<TEntity> _collection;

	public MongoRepository()
	{
		var client = new MongoClient(Environment.GetEnvironmentVariable("MongoDb:ConnectionString"));
        var database = client.GetDatabase(Environment.GetEnvironmentVariable("MongoDb:DatabaseName"));
		_collection = database.GetCollection<TEntity>(Environment.GetEnvironmentVariable($"MongoDb:{nameof(TEntity)}Collection"));
	}

    public async Task<List<TEntity>> GetAllAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<TEntity> GetByIdAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task AddAsync(TEntity newEntity) =>
        await _collection.InsertOneAsync(newEntity);

    public async Task UpdateAsync(TEntity updateEntity) =>
        await _collection.ReplaceOneAsync(x => x.Id == updateEntity.Id, updateEntity);

    public async Task RemoveAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);

    public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> expression)
        => await _collection.Find(expression).ToListAsync();

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        => await _collection.InsertManyAsync(entities);

    public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        => await _collection.DeleteManyAsync(x => entities.Contains(x));

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        foreach(var entity in entities)
        {
            await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        }
    }
}
