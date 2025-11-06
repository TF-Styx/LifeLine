using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.Position
{
    internal sealed class PositionService(HttpClient httpClient, string departmentId) 
        : BaseHttpService<PositionResponse, string>(httpClient, $"api/departments/{departmentId}/positions"), IPositionService;
}
