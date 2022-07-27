using System.Linq.Expressions;

namespace CloudStorage.Core.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task UpdateAsync(TEntity entity);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        Task RemoveAsync(TEntity entity);

        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

        Task<TEntity> GetByIdAsync(params object[] keys);

        Task<List<TEntity>> GetAllAsync();

        List<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression);

        Task<int> SaveChangesAsync();
    }
}
