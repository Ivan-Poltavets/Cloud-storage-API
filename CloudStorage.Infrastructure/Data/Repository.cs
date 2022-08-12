using CloudStorage.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CloudStorage.Infrastructure.Data
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly CosmosDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet; 

        public Repository(CosmosDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity = CheckForNull(entity);
            return (await _dbSet.AddAsync(entity)).Entity;
        }

        public async Task<List<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            CheckForNull(entities);
            await _dbSet.AddRangeAsync(entities);
            return entities.ToList();
        }

        public async Task<List<TEntity>> GetAllAsync()
            => await Task.Run(() => _dbSet.AsNoTracking().ToList());

        public async Task<TEntity> GetByIdAsync(params object[] keys)
            => await _dbSet.FindAsync(keys);

        public List<TEntity> Where(Expression<Func<TEntity, bool>> expression)
            => _dbSet.Where(expression).ToList();

        public async Task<TEntity> RemoveAsync(TEntity entity)
            => await Task.Run(() => _dbSet.Remove(entity).Entity);

        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
            => await Task.Run(() => _dbSet.RemoveRange(entities));

        public async Task<TEntity> UpdateAsync(TEntity entity)
            => await Task.Run(() => _dbSet.Update(entity).Entity);

        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
            => await Task.Run(() => _dbSet.UpdateRange(entities));

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                if(_dbContext.Database.CurrentTransaction != null)
                {
                    _dbContext.Database.CurrentTransaction.Rollback();
                }

                throw;
            }
        }

        private static TEntity CheckForNull(TEntity? entity)
        {
            if(entity == null)
            {
                throw new Exception();
            }
            return entity;
        }
        
        private static IEnumerable<TEntity> CheckForNull(IEnumerable<TEntity> entities)
        {
            if(!entities.Any() || entities == null)
            {
                throw new Exception();
            }
            return entities;
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
            => await _dbSet.SingleOrDefaultAsync(expression);

    }
}
