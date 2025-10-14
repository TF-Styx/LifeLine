using LifeLine.Employee.Service.Domain.Models;
using Shared.Api.Infrastructure;

namespace LifeLine.EmployeeService.Application.Abstraction.Common.Repositories
{
    public interface IEmployeeTypeRepository : IBaseRepository<EmployeeType>
    {
        Task<EmployeeType?> GetByIdAsync(Guid id);
    }
}
