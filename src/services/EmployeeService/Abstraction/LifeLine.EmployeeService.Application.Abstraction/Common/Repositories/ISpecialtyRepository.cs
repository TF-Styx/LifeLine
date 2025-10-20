using LifeLine.Employee.Service.Domain.Models;
using Shared.Api.Infrastructure;

namespace LifeLine.EmployeeService.Application.Abstraction.Common.Repositories
{
    public interface ISpecialtyRepository : IBaseRepository<Specialty>
    {
        Task<Specialty?> GetByIdAsync(Guid id);
    }
}
