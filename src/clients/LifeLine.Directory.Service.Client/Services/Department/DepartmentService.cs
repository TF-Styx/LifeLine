using Shared.Contracts.Request.DirectoryService.Department;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using System.Net.Http.Json;
using System.Text.Json;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Client.Services.Department
{
    public sealed class DepartmentService(HttpClient httpClient) : BaseHttpService<DepartmentResponse, string>(httpClient, "api/departments"), IDepartmentService
    {
        public async Task<Result> UpdateAsync(string departmentId, UpdateDepartmentRequest request)
        {
            var response = await HttpClient.PutAsJsonAsync($"{Url}/{departmentId}", request, JsonSerializerOptions);
            response.EnsureSuccessStatusCode();

            return Result.Success();
        }

        public async Task<Result> ForceDeleteAsync(string id)
        {
            HttpResponseMessage response = null!;

            try
            {
                response = await HttpClient.DeleteAsync($"{Url}/{id}/force-delete");
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                    return Result.Failure(new Error(ErrorCode.Delete, $"Сетевая ошибка удаления элемента в {Url} : {ex.Message}"));

                return Result.Failure(new Error(ErrorCode.Delete, $"Ошибка удаления элемента в {Url} : {response.StatusCode} - {await response.Content.ReadAsStringAsync()}"));
            }
            catch (JsonException jsonEx)
            {
                return Result.Failure(new Error(ErrorCode.Delete, $"Ошибка десериализации ответа от {Url}: {jsonEx.Message}"));
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Delete, $"Непредвиденная ошибка удаления элемента в {Url} : {ex.Message}"));
            }
        }
    }
}
