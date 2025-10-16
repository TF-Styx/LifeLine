using LifeLine.User.Service.Client.Models.Request;
using LifeLine.User.Service.Client.Models.Response;
using Shared.Kernel.Results;

namespace LifeLine.User.Service.Client.Services
{
    public interface IAuthorizationService
    {
        UserResponse? CurrentUser { get; }

        Task<Result<UserResponse?>> AuthAsync(UserRequest request);
    }
}