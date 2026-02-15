using Shared.Client.Security.Abstraction;
using Shared.Client.Security.Utilities;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Client.Security.Cryptography
{
    public class KeyDerivationService : IKeyDerivationService
    {
        public (byte[] Kek, string AuthHash) DeriveKeysFromPassword(string password, byte[] salt)
        {
            byte[] masterKey = Rfc2898DeriveBytes.Pbkdf2(password, salt, SecurityConstants.ITERATIONS, HashAlgorithmName.SHA256, SecurityConstants.KEY_SIZE);
            byte[] emptySalt = [];
            byte[] kek = HKDF.DeriveKey(HashAlgorithmName.SHA256, masterKey, SecurityConstants.KEY_SIZE, emptySalt, Encoding.UTF8.GetBytes("AES-GCM-KEK-v1"));
            byte[] authHashBytes = HKDF.DeriveKey(HashAlgorithmName.SHA256, masterKey, SecurityConstants.KEY_SIZE, emptySalt, Encoding.UTF8.GetBytes("SERVER-AUTH-HASH-v1"));

            Array.Clear(masterKey, 0, masterKey.Length);

            string authHashString = Convert.ToBase64String(authHashBytes);

            return (kek, authHashString);
        }
    }
}
