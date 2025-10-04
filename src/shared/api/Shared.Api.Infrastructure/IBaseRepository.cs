using Shared.Kernel.Primitives;

namespace Shared.Api.Infrastructure
{
    public interface IBaseRepository<TEntity> where TEntity : class, IAggregate
    {
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        void Remove(TEntity entity);
    }
}