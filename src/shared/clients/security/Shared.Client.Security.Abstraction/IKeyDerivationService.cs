namespace Shared.Client.Security.Abstraction
{
    public interface IKeyDerivationService
    {
        (byte[] Kek, string AuthHash) DeriveKeysFromPassword(string password, byte[] salt);
    }
}
