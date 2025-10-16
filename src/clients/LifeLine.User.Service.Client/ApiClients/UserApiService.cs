using LifeLine.User.Service.Client.Models.Request;
using LifeLine.User.Service.Client.Models.Response;
using Shared.Kernel.Results;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace LifeLine.User.Service.Client.ApiClients
{
    public class UserApiService(HttpClient httpClient) : IUserApiService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

        public async Task<Result<UserResponse?>> AuthAsync(UserRequest request, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, "api/users");
            httpRequest.Content = JsonContent.Create(request);

            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return Result<UserResponse?>.Failure(new Error(ErrorCode.InvalidResponse, await response.Content.ReadAsStringAsync(cancellationToken)));

            return Result<UserResponse?>.Success(await response.Content.ReadFromJsonAsync<UserResponse>(cancellationToken));
        }
    }
}
