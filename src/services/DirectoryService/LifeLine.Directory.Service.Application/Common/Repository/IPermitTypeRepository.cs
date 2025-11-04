using LifeLine.Directory.Service.Domain.Models;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Application.Common.Repository
{
    public interface IPermitTypeRepository : IBaseRepository<PermitType>
    {
        Task<PermitType?> GetByIdAsync(Guid id);
    }
}
