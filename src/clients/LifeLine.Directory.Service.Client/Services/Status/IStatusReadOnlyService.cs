using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.Status
{
    public interface IStatusReadOnlyService : IBaseReadHttpService<StatusResponse, string>;
}
