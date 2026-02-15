namespace Shared.Contracts.Response.UserService
{
    public sealed record AuthResponse(string AccessToken, string RefreshToken, string? M2 = null);
}
