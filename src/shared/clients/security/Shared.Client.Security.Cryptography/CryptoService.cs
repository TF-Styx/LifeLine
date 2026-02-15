using Shared.Client.Security.Abstraction;
using Shared.Client.Security.Utilities;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Shared.Client.Security.Cryptography
{
    public class CryptoService : ICryptoService
    {
        public string EncryptedData<T>(T data, byte[] key)
        {
            string jsonString = JsonSerializer.Serialize(data);
            byte[] plainBytes = Encoding.UTF8.GetBytes(jsonString);

            var nonce = new byte[SecurityConstants.NONCE_SIZE];
            RandomNumberGenerator.Fill(nonce);

            var cipherText = new byte[plainBytes.Length];
            var tag = new byte[SecurityConstants.TAG_SIZE];

            using var aes = new AesGcm(key, tag.Length);
            aes.Encrypt(nonce, plainBytes, cipherText, tag);

            var result = new byte[nonce.Length + cipherText.Length + tag.Length];

            Buffer.BlockCopy(nonce, 0, result, 0, nonce.Length);
            Buffer.BlockCopy(cipherText, 0, result, nonce.Length, cipherText.Length);
            Buffer.BlockCopy(tag, 0, result, nonce.Length + cipherText.Length, tag.Length);

            return Convert.ToBase64String(result);
        }

        public T? DecryptedData<T>(string encryptedBase64, byte[] key)
        {
            if (string.IsNullOrEmpty(encryptedBase64)) 
                return default;

            byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);

            if (encryptedBytes.Length < SecurityConstants.NONCE_SIZE + SecurityConstants.TAG_SIZE)
                throw new ArgumentException("Недопустимый формат зашифрованных данных");

            var nonce = encryptedBytes.AsSpan(0, SecurityConstants.NONCE_SIZE);
            var tag = encryptedBytes.AsSpan(encryptedBytes.Length - SecurityConstants.TAG_SIZE, SecurityConstants.TAG_SIZE);
            var cipherText = encryptedBytes.AsSpan(SecurityConstants.NONCE_SIZE, encryptedBytes.Length - SecurityConstants.NONCE_SIZE - SecurityConstants.TAG_SIZE);

            var plainBytes = new byte[cipherText.Length];

            using var aes = new AesGcm(key, tag.Length);
            aes.Decrypt(nonce, cipherText, tag, plainBytes);

            string jsonString = Encoding.UTF8.GetString(plainBytes);

            return JsonSerializer.Deserialize<T>(jsonString);
        }

        public byte[] GenerateRandomBytes(int length = 32)
        {
            var bytes = new byte[length];
            RandomNumberGenerator.Fill(bytes);
            return bytes;
        }
    }
}
