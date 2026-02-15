namespace Shared.Contracts.Request.UserService.SRP
{
    public sealed record SRPVerifyRequest(string Login, string A, string M1);
}
