using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using System.Net.Http.Json;

namespace LifeLine.Directory.Service.Client.Services.Position
{
    internal sealed class PositionService(HttpClient httpClient, string departmentId) 
        : BaseHttpService<PositionResponse, string>(httpClient, $"api/departments/{departmentId}/positions"), IPositionService
    {
        public async Task<List<PositionResponse>> GetAllPosition()
        {
            var response = await HttpClient.GetAsync($"{Url}/get-all");
            var result = await response.Content.ReadFromJsonAsync<List<PositionResponse>>(JsonSerializerOptions);

            return result;
        }
    }
}
