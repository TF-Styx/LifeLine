using LifeLine.Directory.Service.Domain.Models;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Application.Common.Repository
{
    public interface IDepartmentRepository : IBaseRepository<Department>
    {
        Task<Department?> GetByIdAsync(Guid id);
    }
}
