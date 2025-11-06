using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Client.Services.Department
{
    public interface IDepartmentService : IDepartmentReadOnlyService, IBaseWriteHttpService<DepartmentResponse, string>
    {
        Task<Result> ForceDeleteAsync(string id);
    }
}
