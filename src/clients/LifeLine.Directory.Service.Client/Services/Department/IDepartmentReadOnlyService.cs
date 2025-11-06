using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.Department
{
    public interface IDepartmentReadOnlyService : IBaseReadHttpService<DepartmentResponse, string>;
}
