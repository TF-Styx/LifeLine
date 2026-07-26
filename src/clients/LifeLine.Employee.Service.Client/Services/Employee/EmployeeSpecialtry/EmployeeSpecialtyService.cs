using Microsoft.Extensions.Options;
using Shared.Contracts.Request.EmployeeService.EmployeeSpecialty;
using Shared.Http.Base;
using Shared.Kernel.Errors;
using System.Net.Http.Json;
using System.Text.Json;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.EmployeeSpecialtry
{
    internal sealed class EmployeeSpecialtyService(HttpClient httpClient, string employeeId, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<HttpClient, string>(httpClient, $"api/employees/{employeeId}/employee-specialties", options.Value), IEmployeeSpecialtyService
    {
        public async Task<Result> CreateManyAsync(CreateManyEmployeeSpecialtiesRequest request)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync($"{Url}/batch", request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(AppErrors.CreateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.UpdateHttp, $"Произошла ошибка при добавлении специальности!\n{ex}"));
            }
        }

        public async Task<Result> UpdateEmployeeSpecialtyAsync(UpdateEmployeeSpecialtyRequest request)
        {
            try
            {
                var response = await HttpClient.PatchAsJsonAsync($"{Url}", request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(AppErrors.UpdateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.UpdateHttp, $"Произошла ошибка при изменении специальности!\n{ex}"));
            }
        }

        public async Task<Result> DeleteEmployeeSpecialtyAsync(string specialtyId)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"{Url}/{specialtyId}");

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(AppErrors.DeleteHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.DeleteHttp, $"Произошла ошибка при удалении специальности!\n{ex}"));
            }
        }
    }
}
