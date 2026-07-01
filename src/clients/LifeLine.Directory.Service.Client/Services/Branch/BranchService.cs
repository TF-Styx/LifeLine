using Shared.Http.Base;
using System.Net.Http.Json;
using Terminex.Common.Results;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Client.Services.Branch
{
    public sealed class BranchService(HttpClient httpClient) : BaseHttpService<BranchResponse, string>(httpClient, "api/branches"), IBranchService
    {
        public async Task<Result<List<BranchResponse>>> GetAllByIdHospitalAsync(string hospitalId)
        {
			try
			{
                var response = await HttpClient.GetAsync($"{Url}/by-hospital-id/{hospitalId}");
                response.EnsureSuccessStatusCode();

                var branches = await response.Content.ReadFromJsonAsync<List<BranchResponse>>(JsonSerializerOptions);

                return Result<List<BranchResponse>>.Success(branches ?? []);
            }
			catch (Exception ex)
			{
                return Result<List<BranchResponse>>.Failure(Error.BadRequest($"Не валидный запрос!\n" +
                                                                             $"Исключение: {ex}\n"));
			}
        }
    }
}
