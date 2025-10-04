namespace Shared.Api.Infrastructure
{
    public interface IBaseReadDbContext
    {
        IQueryable<TEntity> Set<TEntity>() where TEntity : class;
    }
}
