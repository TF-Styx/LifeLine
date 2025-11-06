using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.Status
{
    public sealed class StatusService(HttpClient httpClient) : BaseHttpService<StatusResponse, string>(httpClient, "api/statuses"), IStatusService;
}
