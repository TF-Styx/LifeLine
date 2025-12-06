using Shared.Contracts.Request.EmployeeService.ContactInformation;
using Shared.Contracts.Request.EmployeeService.Employee;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using Shared.Kernel.Results;
using System.Net.Http.Json;

namespace LifeLine.Employee.Service.Client.Services.Employee
{
    public sealed class EmployeeService(HttpClient httpClient) : BaseHttpService<EmployeeResponse, string>(httpClient, "api/employees"), IEmployeeService
    {
        public async Task<List<EmployeeHrItemResponse>> GetAllForHrAsync()
        {
			try
			{
				var response = await HttpClient.GetAsync($"{Url}/get-all-for-hr");
				response.EnsureSuccessStatusCode();

				return await response.Content.ReadFromJsonAsync<List<EmployeeHrItemResponse>>(JsonSerializerOptions) ?? [];
			}
			catch (Exception)
			{
				return [];
			}
        }

		public async Task<EmployeeFullDetailsResponse?> GetDetailsAsync(string id)
		{
			try
			{
				var response = await HttpClient.GetAsync($"{Url}/{id}/get-full-details-for-employee");
				response.EnsureSuccessStatusCode();

				return await response.Content.ReadFromJsonAsync<EmployeeFullDetailsResponse>();
			}
			catch (Exception)
			{
				return null;
			}
		}

		public async Task<Result> UpdateEmployeeAsync(string employeeId, UpdateEmployeeRequest request)
		{
			try
			{
				var response = await HttpClient.PatchAsJsonAsync($"{Url}/{employeeId}/update-employee", request, JsonSerializerOptions);
                //response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(ErrorCode.UpdateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
			catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.UpdateHttp, $"Произошла ошибка при изменении данных пользователя!\n{ex}"));
            }
		}
    }
}
