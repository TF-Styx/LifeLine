using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.PermitType
{
    public interface IPermitTypeReadOnlyService : IBaseReadHttpService<PermitTypeResponse, string>;
}
