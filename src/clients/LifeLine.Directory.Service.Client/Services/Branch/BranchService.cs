using Microsoft.Extensions.Options;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using System.Net.Http.Json;
using System.Text.Json;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Client.Services.Branch
{
    public sealed class BranchService(HttpClient httpClient, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<BranchResponse, string>(httpClient, "api/branches", options.Value), IBranchService
    {
        public async Task<Result<List<BranchResponse>>> GetAllByHospitalIdAsync(string hospitalId)
        {
			try
			{
                var response = await HttpClient.GetAsync($"{Url}/hospital/{hospitalId}");
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
