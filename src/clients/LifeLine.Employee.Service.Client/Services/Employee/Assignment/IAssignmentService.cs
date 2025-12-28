using Shared.Contracts.Request.EmployeeService.Assignment;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.Assignment
{
    public interface IAssignmentService : IBaseHttpService<AssignmentResponse, string>
    {
        Task<Result> UpdateAssignmentAsync(Guid assignmentId, Guid contractId, UpdateAssignmentRequest request);
        Task<Result> DeleteAssignmentContractAsync(Guid assignmentId);
    }
}