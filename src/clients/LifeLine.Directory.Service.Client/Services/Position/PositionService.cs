using Shared.Contracts.Request.DirectoryService.Position;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using System.Net.Http.Json;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Client.Services.Position
{
    internal sealed class PositionService(HttpClient httpClient, string departmentId) 
        : BaseHttpService<PositionResponse, string>(httpClient, $"api/departments/{departmentId}/positions"), IPositionService
    {
        public async Task<Result> UpdateAsync(string positionId, UpdatePositionRequest request)
        {
            var response = await HttpClient.PutAsJsonAsync($"{Url}/{positionId}", request, JsonSerializerOptions);
            response.EnsureSuccessStatusCode();

            return Result.Success();
        }

        public async Task<List<PositionResponse>> GetAllPosition()
        {
            var response = await HttpClient.GetAsync($"{Url}/get-all");
            var result = await response.Content.ReadFromJsonAsync<List<PositionResponse>>(JsonSerializerOptions);

            return result!;
        }
    }
}
