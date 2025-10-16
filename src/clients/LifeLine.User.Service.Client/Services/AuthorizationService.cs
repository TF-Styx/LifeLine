using LifeLine.User.Service.Client.ApiClients;
using LifeLine.User.Service.Client.Models.Request;
using LifeLine.User.Service.Client.Models.Response;
using Shared.Kernel.Results;

namespace LifeLine.User.Service.Client.Services
{
    public class AuthorizationService(IUserApiService userApiService) : IAuthorizationService
    {
        private readonly IUserApiService _userApiService = userApiService;

        public UserResponse? CurrentUser { get; private set; }

        public async Task<Result<UserResponse?>> AuthAsync(UserRequest request)
        {
            var result = await _userApiService.AuthAsync(request);

            if (result.IsSuccess)
                CurrentUser = result.Value;

            return result;
        }
    }
}
