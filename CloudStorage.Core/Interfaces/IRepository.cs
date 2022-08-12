using System.Linq.Expressions;

namespace CloudStorage.Core.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task<List<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        Task<TEntity> RemoveAsync(TEntity entity);

        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

        Task<TEntity> GetByIdAsync(params object[] keys);

        Task<List<TEntity>> GetAllAsync();

        List<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression);

        Task<int> SaveChangesAsync();
    }
}
