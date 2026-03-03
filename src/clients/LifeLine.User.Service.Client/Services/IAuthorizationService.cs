using Terminex.Common.Results;

namespace LifeLine.User.Service.Client.Services
{
    public interface IAuthorizationService
    {
        Task<Result> AuthAsync(string login, string password);
        Task LogoutAsync();
    }
}