using Shared.Contracts.Request.EmployeeService.WorkPermit;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.WorkPermit
{
    public interface IWorkPermitService : IBaseHttpService<WorkPermitResponse, string>
    {
        Task<Result> UpdateWorkPermitAsync(Guid workPermitId, UpdateWorkPermitRequest request);
        Task<Result> DeleteWorkPermitAsync(Guid workPermitId);
    }
}