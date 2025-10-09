using LifeLine.Directory.Service.Domain.Models;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Application.Common.Repository
{
    public interface IStatusRepository : IBaseRepository<Status>
    {
        Task<Status?> GetByIdAsync(Guid id);
    }
}
