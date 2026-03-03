using Shared.Contracts.Request.EmployeeService.EmployeeSpecialty;
using Shared.Http.Base;
using System.Net.Http.Json;
using Terminex.Common.Results;
using Shared.Kernel.Errors;

namespace LifeLine.Employee.Service.Client.Services.Employee.EmployeeSpecialtry
{
    internal sealed class EmployeeSpecialtyService(HttpClient httpClient, string employeeId) 
        : BaseHttpService<HttpClient, string>(httpClient, $"api/employees/{employeeId}/employee-specialties"), IEmployeeSpecialtyService
    {
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

        public async Task<Result> DeleteEmployeeSpecialtyAsync(Guid specialtyId)
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
