using Shared.Api.Infrastructure;
using Shared.Domain.ValueObjects;
using Terminex.Common.Results;

namespace LifeLine.EmployeeService.Application.Abstraction.Common.Repositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee.Service.Domain.Models.Employee>
    {
        Task<Employee.Service.Domain.Models.Employee?> GetByIdAsync(Guid id);
        Task<Result> HasActiveAssignmentsToDepartmentAsync(DepartmentId departmentId, CancellationToken cancellationToken = default);
        Task<Result> HasActiveAssignmentsToPositionAsync(PositionId positionId, CancellationToken cancellationToken = default);
    }
}
