using LifeLine.User.Service.Client.ApiClients;
using Shared.Client.Security.Abstraction;
using Shared.Contracts.Desktop;
using Shared.Contracts.Request.UserService.SRP;
using Shared.Kernel.Results;
using System.IdentityModel.Tokens.Jwt;

namespace LifeLine.User.Service.Client.Services
{
    public class AuthorizationService
        (
            IUserApiService userApiService, 
            ISRPService srpService, 
            ITokenStorage tokenStorage, 
            IUserContext userContext
        ) : IAuthorizationService
    {
        private readonly IUserApiService _userApiService = userApiService;
        private readonly ISRPService _srpService = srpService;
        private readonly ITokenStorage _tokenStorage = tokenStorage;
        private readonly IUserContext _userContext = userContext;

        public async Task<Result> AuthAsync(string login, string password)
        {
            var srpChallengeResult = await _userApiService.SRPChallengeAsync(new SRPChallengeRequest(login));

            if (srpChallengeResult.IsFailure)
                return Result.Failure(srpChallengeResult.Errors);

            var (A, M1, S) = _srpService.GenerateSrpProof(password, srpChallengeResult.Value.Salt, srpChallengeResult.Value.B);

            var verifyResult = await _userApiService.VerifySRPAsync(new SRPVerifyRequest(login, A, M1));

            if (verifyResult.IsFailure)
                return Result.Failure(verifyResult.Errors);

            await _tokenStorage.SaveAsync(verifyResult.Value.AccessToken, verifyResult.Value.RefreshToken);

            bool serverVerification = _srpService.VerifyServerM2(A, M1, S, verifyResult.Value.M2!);

            if (!serverVerification)
                return Result.Failure(new Error(ErrorCode.SRPVerificationFailed, "Сервер не прошел верификацию!"));

            _userContext.SetUser(new CurrentUser(ExtractUserIdFromAccessToken(verifyResult.Value.AccessToken).ToString()));

            return Result.Success();
        }

        public async Task LogoutAsync()
        {
            await _tokenStorage.ClearAsync();
            _userContext.Clear();
        }

        private Guid ExtractUserIdFromAccessToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);

            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            var jtiClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);

            if (userIdClaim?.Value == null || jtiClaim?.Value == null)
                throw new InvalidOperationException("У Access токена отсутствуют необходимые значения.");

            if (!Guid.TryParse(userIdClaim.Value, out var userId))
                throw new InvalidOperationException("Неверный идентификатор пользователя в токене.");

            return userId;
        }
    }
}
