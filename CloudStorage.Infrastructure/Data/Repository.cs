using CloudStorage.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CloudStorage.Infrastructure.Data
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly AuthDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet; 

        public Repository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            entity = CheckForNull(entity);
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            CheckForNull(entities);
            await _dbSet.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TEntity>> GetAllAsync()
            => await Task.Run(() => _dbSet.AsNoTracking().ToList());

        public TEntity GetById(params object[] keys)
            => _dbSet.Find(keys);

        public List<TEntity> Where(Expression<Func<TEntity, bool>> expression)
            => _dbSet.Where(expression).ToList();

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public TEntity Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public List<TEntity> UpdateRange(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
            _dbContext.SaveChanges();
            return entities.ToList();
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

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.SingleOrDefault(expression);
        }
    }
}
