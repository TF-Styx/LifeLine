using Microsoft.Extensions.Options;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using System.Text.Json;

namespace LifeLine.Directory.Service.Client.Services.Status
{
    public sealed class StatusService(HttpClient httpClient, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<StatusResponse, string>(httpClient, "api/statuses", options.Value), IStatusService;
}
