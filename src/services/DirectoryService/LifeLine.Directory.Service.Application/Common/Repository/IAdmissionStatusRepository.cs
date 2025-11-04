using LifeLine.Directory.Service.Domain.Models;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Application.Common.Repository
{
    public interface IAdmissionStatusRepository : IBaseRepository<AdmissionStatus>
    {
        Task<AdmissionStatus?> GetByIdAsync(Guid id);
    }
}
