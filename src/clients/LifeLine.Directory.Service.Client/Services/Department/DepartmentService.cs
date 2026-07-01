using Shared.Http.Base;
using System.Net.Http.Json;
using Terminex.Common.Results;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Client.Services.Department
{
    public sealed class DepartmentService(HttpClient httpClient) : BaseHttpService<DepartmentResponse, string>(httpClient, "api/departments"), IDepartmentService
    {
        public async Task<Result<List<DepartmentResponse>>> GetAllByBranchIdAsync(string branchId)
		{
			try
            {
                var response = await HttpClient.GetAsync($"{Url}/by-branch-id/{branchId}");
                response.EnsureSuccessStatusCode();

                var departments = await response.Content.ReadFromJsonAsync<List<DepartmentResponse>>(JsonSerializerOptions);

                return Result<List<DepartmentResponse>>.Success(departments ?? []);
            }
			catch (Exception ex)
            {
                return Result<List<DepartmentResponse>>.Failure(Error.BadRequest($"Не валидный запрос!\n" +
                                                                                 $"Исключение: {ex}\n"));
            }
        }
    }
}
