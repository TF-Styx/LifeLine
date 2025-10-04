using Microsoft.EntityFrameworkCore;

namespace Shared.Api.Infrastructure
{
    public interface IBaseWriteDbContext : IUnitOfWork
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
