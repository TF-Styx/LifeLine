namespace Shared.Contracts.Request.UserService
{
    public sealed record LoginByTokenRequest(string RefreshToken, string? AccessToken = null);
}
