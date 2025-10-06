using LifeLine.Employee.Service.Domain.Models;
using Shared.Api.Infrastructure;

namespace LifeLine.EmployeeService.Application.Abstraction.Common.Repositories
{
    public interface IGenderRepository : IBaseRepository<Gender>
    {
        Task<bool> ExistNameAsync(string name);
        Task<Gender?> GetByIdAsync(Guid id);
    }
}
