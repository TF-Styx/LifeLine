using Shared.Contracts.Desktop;
using Shared.Kernel.Results;

namespace LifeLine.User.Service.Client.Services
{
    public interface IAuthorizationService
    {
        Task<Result> AuthAsync(string login, string password);
    }
}