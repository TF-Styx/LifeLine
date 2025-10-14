using LifeLine.Employee.Service.Domain.Models;
using Shared.Api.Infrastructure;

namespace LifeLine.EmployeeService.Application.Abstraction.Common.Repositories
{
    public interface IAssignmentRepository : IBaseRepository<Assignment>
    {
        Task<Assignment?> GetByIdAsync(Guid id);
    }
}
