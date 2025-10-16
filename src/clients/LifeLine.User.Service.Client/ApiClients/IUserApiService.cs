using LifeLine.User.Service.Client.Models.Request;
using LifeLine.User.Service.Client.Models.Response;
using Shared.Kernel.Results;

namespace LifeLine.User.Service.Client.ApiClients
{
    public interface IUserApiService
    {
        Task<Result<UserResponse?>> AuthAsync(UserRequest request, CancellationToken cancellationToken = default);
    }
}