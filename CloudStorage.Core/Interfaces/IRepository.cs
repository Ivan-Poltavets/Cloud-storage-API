namespace CloudStorage.Core.Interfaces;

public interface IRepository<TEntity>
{
    Task<TEntity> GetByIdAsync(string id);
    Task<List<TEntity>> GetAllAsync();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(string id);
}
