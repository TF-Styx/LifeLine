using LifeLine.HrPanel.Desktop.Services.Secure;
using LifeLine.User.Service.Client.ApiClients;
using Shared.Client.Security.Abstraction;
using Shared.Contracts.Request.UserService;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LifeLine.HrPanel.Desktop.Handlers
{
    public sealed class AttachTokenHandler
        (
            ITokenStorage tokenStorage,
            IUserApiService authService,
            IAuthenticationStateService authenticationStateService
        ) : DelegatingHandler
    {
        private readonly SemaphoreSlim semaphoreSlim = new (1, 1);

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await tokenStorage.GetAccessTokenAsync();
            var refreshToken = await tokenStorage.GetRefrashTokenAsync();

            if (!string.IsNullOrWhiteSpace(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await semaphoreSlim.WaitAsync(cancellationToken);

                try
                {
                    var currentAccessToken = await tokenStorage.GetAccessTokenAsync();
                    var currentRefreshToken = await tokenStorage.GetRefrashTokenAsync();

                    if (currentAccessToken != accessToken && !string.IsNullOrWhiteSpace(currentAccessToken))
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        return await base.SendAsync(request, cancellationToken);
                    }

                    if (string.IsNullOrWhiteSpace(refreshToken))
                    {
                        await tokenStorage.ClearAsync();
                        authenticationStateService.NotifyAuthenticationRequired();
                        return response;
                    }

                    var result = await authService.RefreshToken(new LoginByTokenRequest(refreshToken, currentAccessToken));

                    if (result.IsSuccess)
                    {
                        await tokenStorage.SaveAsync(result.Value.AccessToken, result.Value.AccessToken);

                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.Value.AccessToken);

                        return await base.SendAsync(request, cancellationToken);
                    }
                    else
                    {
                        await tokenStorage.ClearAsync();
                        authenticationStateService.NotifyAuthenticationRequired();
                        return response;
                    }
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }

            return response;
        }
    }
}
