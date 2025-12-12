using Shared.Contracts.Request.EmployeeService.EmployeeSpecialty;
using Shared.Http.Base;
using Shared.Kernel.Results;
using System.Net.Http.Json;

namespace LifeLine.Employee.Service.Client.Services.Employee.EmployeeSpecialtry
{
    internal sealed class EmployeeSpecialtyService(HttpClient httpClient, string employeeId) 
        : BaseHttpService<HttpClient, string>(httpClient, $"api/employees/{employeeId}/employee-specialties"), IEmployeeSpecialtyService
    {
        public async Task<Result> UpdateEmployeeSpecialtyAsync(Guid employeeId, UpdateEmployeeSpecialtyRequest request)
        {
            try
            {
                var response = await HttpClient.PatchAsJsonAsync($"{Url}", request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(ErrorCode.UpdateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.UpdateHttp, $"Произошла ошибка при изменении специальности!\n{ex}"));
            }
        }
    }
}
