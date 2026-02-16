namespace Shared.Client.Security.Abstraction
{
    public interface ITokenStorage
    {
        Task<string?> GetAccessTokenAsync();
        Task<string?> GetRefrashTokenAsync();
        Task SaveAsync(string accessToken, string refrashToken);
        Task ClearAsync();
    }
}
