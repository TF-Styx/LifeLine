using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Primitives;

namespace Shared.Api.Infrastructure
{
    public abstract class BaseRepository<TEntity, TContext>(TContext context) : IBaseRepository<TEntity>
        where TEntity : class, IAggregate
        where TContext : IBaseWriteDbContext
    {
        protected readonly TContext _context = context;
        protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);

            return entity;
        }

        public virtual void Remove(TEntity entity) => _dbSet.Remove(entity); 
    }
}
