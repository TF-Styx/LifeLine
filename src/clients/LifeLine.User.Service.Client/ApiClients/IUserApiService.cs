using Shared.Contracts.Request.UserService.SRP;
using Shared.Contracts.Response.UserService;
using Terminex.Common.Results;

namespace LifeLine.User.Service.Client.ApiClients
{
    public interface IUserApiService
    {
        Task<Result<SRPChallengeResponse>> SRPChallengeAsync(SRPChallengeRequest request);
        Task<Result<AuthResponse>> VerifySRPAsync(SRPVerifyRequest request);
    }
}