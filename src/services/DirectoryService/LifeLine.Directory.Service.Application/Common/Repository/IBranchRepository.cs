using LifeLine.Directory.Service.Domain.Models;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Application.Common.Repository
{
    public interface IBranchRepository : IBaseRepository<Branch>
    {
        Task<Branch?> GetByIdAsync(Guid id);
    }
}
