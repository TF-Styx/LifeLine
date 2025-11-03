using LifeLine.Directory.Service.Domain.Models;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Application.Common.Repository
{
    public interface IEducationLevelRepository : IBaseRepository<EducationLevel>
    {
        Task<EducationLevel> GetByIdAsync(Guid id);
    }
}
