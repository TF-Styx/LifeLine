using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.PermitType
{
    public sealed class PermitTypeService(HttpClient httpClient) : BaseHttpService<PermitTypeResponse, string>(httpClient, "api/permit-types"), IPermitTypeService;
}
