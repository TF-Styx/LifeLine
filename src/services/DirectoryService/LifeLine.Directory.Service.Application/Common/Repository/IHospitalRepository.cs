using LifeLine.Directory.Service.Domain.Models;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Application.Common.Repository
{
    public interface IHospitalRepository : IBaseRepository<Hospital>
    {
        Task<Hospital?> GetByIdAsync(Guid id);
    }
}
