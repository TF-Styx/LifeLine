using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
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
    }
}
