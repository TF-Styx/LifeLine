using Microsoft.Extensions.Options;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using System.Net.Http.Json;
using System.Text.Json;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Client.Services.Department
{
    public sealed class DepartmentService(HttpClient httpClient, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<DepartmentResponse, string>(httpClient, "api/departments", options.Value), IDepartmentService
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
