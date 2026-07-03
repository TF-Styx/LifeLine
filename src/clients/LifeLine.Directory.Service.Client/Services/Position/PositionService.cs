using Microsoft.Extensions.Options;
using Shared.Contracts.Request.DirectoryService.Position;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using Shared.Kernel.Errors;
using System.Net.Http.Json;
using System.Text.Json;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Client.Services.Position
{
    internal sealed class PositionService(HttpClient httpClient, string departmentId, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<PositionResponse, string>(httpClient, $"api/departments/{departmentId}/positions", options.Value), IPositionService
    {
        public async Task<Result> UpdateAsync(string positionId, UpdatePositionRequest request)
        {
            try
            {
                var response = await HttpClient.PutAsJsonAsync($"{Url}/{positionId}", request, JsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.CreateHttp, $"Произошла ошибка при сохранении должности!\n{ex}"));
            }
        }

        public async Task<List<PositionResponse>> GetAllPosition()
        {
            var response = await HttpClient.GetAsync($"{Url}/get-all");
            var result = await response.Content.ReadFromJsonAsync<List<PositionResponse>>(JsonSerializerOptions);

            return result!;
        }
    }
}
