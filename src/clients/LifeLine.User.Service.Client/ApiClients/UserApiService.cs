using Shared.Contracts.Request.UserService.SRP;
using Shared.Contracts.Response.UserService;
using Terminex.Common.Results;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Shared.Contracts.Request.UserService;
using Shared.Kernel.Errors;

namespace LifeLine.User.Service.Client.ApiClients
{
    public class UserApiService(HttpClient httpClient) : IUserApiService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public async Task<Result<SRPChallengeResponse>> SRPChallengeAsync(SRPChallengeRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/srp/challenge", request, _jsonSerializerOptions);

            if (!response.IsSuccessStatusCode)
                return Result<SRPChallengeResponse>.Failure(new Error(ErrorCode.InvalidResponse, await response.Content.ReadAsStringAsync()));

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<SRPChallengeResponse>(content, _jsonSerializerOptions);

            return Result<SRPChallengeResponse>.Success(result!);
        }

        public async Task<Result<AuthResponse>> VerifySRPAsync(SRPVerifyRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/srp/verify", request, _jsonSerializerOptions);

            if (!response.IsSuccessStatusCode)
                return Result<AuthResponse>.Failure(new Error(ErrorCode.InvalidResponse, await response.Content.ReadAsStringAsync()));

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<AuthResponse>(content, _jsonSerializerOptions);

            return Result<AuthResponse>.Success(result!);
        }

        public async Task<Result<AuthResponse>> RefreshToken(LoginByTokenRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/refresh-token", request, _jsonSerializerOptions);
                response.EnsureSuccessStatusCode();
                
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponse>(_jsonSerializerOptions);

                return authResponse;
            }
            catch (HttpRequestException ex)
            {
                return Error.New(ErrorCode.BadRequest, $"Ошибка! - {ex}");
            }
			catch (Exception ex)
			{
                return Result<AuthResponse>.Failure(Error.BadRequest($"Не валидный запрос!\n" +
                                                                     $"Исключение: {ex}\n"));
			}
        }
    }
}
