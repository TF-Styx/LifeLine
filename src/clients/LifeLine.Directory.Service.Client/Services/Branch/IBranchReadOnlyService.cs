using Shared.Http.Base;
using Terminex.Common.Results;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Client.Services.Branch
{
    public interface IBranchReadOnlyService : IBaseReadHttpService<BranchResponse, string>
    {
        Task<Result<List<BranchResponse>>> GetAllByHospitalIdAsync(string hospitalId);
    }
}
