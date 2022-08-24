using CloudStorage.Core.Entities;
using System.Linq.Expressions;

namespace CloudStorage.Core.Interfaces;

public interface IMongoRepository<TEntity> : IRepository<TEntity>
    where TEntity : IEntity
{
    Task AddRangeAsync(IEnumerable<TEntity> entities);
    Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task<List<TEntity>> Find(Expression<Func<TEntity, bool>> expression);
}
