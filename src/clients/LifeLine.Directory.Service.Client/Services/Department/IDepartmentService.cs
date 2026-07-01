using Shared.Http.Base;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Client.Services.Department
{
    public interface IDepartmentService : IDepartmentReadOnlyService, IBaseWriteHttpService<DepartmentResponse, string>;
}
