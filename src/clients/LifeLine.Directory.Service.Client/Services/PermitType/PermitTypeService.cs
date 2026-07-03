using Microsoft.Extensions.Options;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using System.Text.Json;

namespace LifeLine.Directory.Service.Client.Services.PermitType
{
    public sealed class PermitTypeService(HttpClient httpClient, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<PermitTypeResponse, string>(httpClient, "api/permit-types", options.Value), IPermitTypeService;
}
