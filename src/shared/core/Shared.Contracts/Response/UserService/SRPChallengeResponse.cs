namespace Shared.Contracts.Response.UserService
{
    public sealed record SRPChallengeResponse(string Salt, string B);
}
