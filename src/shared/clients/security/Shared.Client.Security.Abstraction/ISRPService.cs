namespace Shared.Client.Security.Abstraction
{
    public interface ISRPService
    {
        string GenerateSrpVerifier(string authHash);
        (string A, string M1, string S) GenerateSrpProof(string password, string saltBase64, string B_base64);
        bool VerifyServerM2(string A, string M1, string S, string serverM2);
    }
}
