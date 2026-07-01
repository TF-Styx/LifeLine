using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.Assignment
{
    public interface IAssignmentCheckService
    {
        Task<Result> HasActiveAssignmentsToDepartmentAsync(Guid departmentId, CancellationToken cancellationToken = default);
        Task<Result> HasActiveAssignmentsToPositionAsync(Guid positionId, CancellationToken cancellationToken = default);
    }
}
