using Shared.Api.Infrastructure;

namespace LifeLine.EmployeeService.Application.Abstraction.Common.Repositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee.Service.Domain.Models.Employee>
    {
        Task<Employee.Service.Domain.Models.Employee?> GetByIdAsync(Guid id);
    }
}
