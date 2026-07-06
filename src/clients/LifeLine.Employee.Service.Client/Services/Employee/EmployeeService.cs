using Shared.Contracts.Request.EmployeeService.Employee;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using System.Net.Http.Json;
using Terminex.Common.Results;
using Shared.Kernel.Errors;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LifeLine.Employee.Service.Client.Services.Employee
{
    public sealed class EmployeeService(HttpClient httpClient, IOptions<JsonSerializerOptions> options) 
		: BaseHttpService<EmployeeResponse, string>(httpClient, "api/employees", options.Value), IEmployeeService
    {
		public async Task<Result> AddPersonalPhoto(string employeeId, AddPersonalPhotoRequest request)
		{
			try
			{
				var response = await HttpClient.PatchAsJsonAsync($"{Url}/photo/{employeeId}", request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(AppErrors.CreateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.CreateHttp, $"Произошла ошибка при сохранении персональной фотографии сотрудника!\n{ex}"));
            }
        }

		public async Task<Result> DeletePersonalPhoto(string employeeId)
		{
			try
			{
				var response = await HttpClient.DeleteAsync($"{Url}/photo/{employeeId}");
				response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.CreateHttp, $"Произошла ошибка при удалении персональной фотографии сотрудника!\n{ex}"));
            }
        }

		public async Task<List<EmployeeHrItemResponse>> GetAllForHrAsync()
        {
			try
			{
				var response = await HttpClient.GetAsync($"{Url}/hr");
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
				var response = await HttpClient.GetAsync($"{Url}/details/{id}");
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
				var response = await HttpClient.PatchAsJsonAsync($"{Url}/employee/{employeeId}", request, JsonSerializerOptions);
                //response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(AppErrors.UpdateHttp, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
			catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.UpdateHttp, $"Произошла ошибка при изменении данных пользователя!\n{ex}"));
            }
		}

		public async Task<Result> SoftDeleteAsync(string employeeId)
		{
			try
			{
				var response = await HttpClient.PatchAsync($"{Url}/soft-delete/{employeeId}", null);

				if (!response.IsSuccessStatusCode)
					return Result.Failure(new Error(AppErrors.DeleteHttp, await response.Content.ReadAsStringAsync()));

				return Result.Success();
            }
			catch (Exception ex)
            {
                return Result.Failure(new Error(AppErrors.UpdateHttp, $"Произошла ошибка при деактивации пользователя!\n{ex}"));
            }
		}
    }
}
