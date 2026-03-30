using Shared.Contracts.Request.EmployeeService.WorkPermit;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using System.Net.Http.Json;
using Terminex.Common.Results;
using Shared.Kernel.Errors;

namespace LifeLine.Employee.Service.Client.Services.Employee.WorkPermit
{
    public sealed class WorkPermitService(HttpClient httpClient, string employeeId) :
        BaseHttpService<WorkPermitResponse, string>(httpClient, $"api/employees/{employeeId}/work-permits"), IWorkPermitService
    {
        public async Task<Result> CreateManyAsync(CreateManyWorkPermitsRequest request)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"{Url}/many", request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(AppErrors.CreateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.UpdateHttp, $"Произошла ошибка при сохранении данных в разрешении на работу!\n{ex}"));
            }
        }

        public async Task<Result> UpdateWorkPermitAsync(Guid workPermitId, UpdateWorkPermitRequest request)
        {
            try
            {
                var response = await HttpClient.PatchAsJsonAsync($"{Url}/{workPermitId}", request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(AppErrors.UpdateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.UpdateHttp, $"Произошла ошибка при изменении данных в разрешении на работу!\n{ex}"));
            }
        }

        public async Task<Result> DeleteWorkPermitAsync(Guid workPermitId)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"{Url}/{workPermitId}");

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(AppErrors.DeleteHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.DeleteHttp, $"Произошла ошибка при удалении данных в разрешении на работу!\n{ex}"));
            }
        }
    }
}
