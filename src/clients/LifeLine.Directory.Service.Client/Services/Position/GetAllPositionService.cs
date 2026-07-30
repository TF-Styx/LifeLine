using Microsoft.Extensions.Options;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using System.Text.Json;

namespace LifeLine.Directory.Service.Client.Services.Position
{
    public sealed class GetAllPositionService(HttpClient httpClient, IOptions<JsonSerializerOptions> options)
        : BaseReadHttpService<PositionResponse, string>(httpClient, $"api/positions/get-all", options.Value), IGetAllPositionService;
}
