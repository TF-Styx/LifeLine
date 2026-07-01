using Shared.Http.Base;
using Terminex.Common.Results;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Client.Services.Department
{
    public interface IDepartmentReadOnlyService : IBaseReadHttpService<DepartmentResponse, string>
    {
        Task<Result<List<DepartmentResponse>>> GetAllByBranchIdAsync(string branchId);
    }
}
