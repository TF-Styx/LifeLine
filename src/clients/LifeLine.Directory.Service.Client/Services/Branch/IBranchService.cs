using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.Branch
{
    public interface IBranchService : IBranchReadOnlyService, IBaseWriteHttpService<BranchResponse, string>;
}
