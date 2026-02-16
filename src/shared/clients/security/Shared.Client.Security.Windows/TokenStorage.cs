using Shared.Client.Security.Abstraction;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Shared.Client.Security.Windows
{
    public sealed class TokenStorage : ITokenStorage
    {
        private sealed record Tokens(string AccessToken, string RefrashToken);

        private static readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LifeLine");
        private static readonly string _tokenFilePath = Path.Combine(_filePath, "token.data");
        private static readonly string _entropyFilePath = Path.Combine(_filePath, "entropy.bin");

        private byte[]? _entropy;

        public async Task<string?> GetAccessTokenAsync()
        {
            var tokens = await ReadAndDecryptTokensAsync();

            return tokens?.AccessToken;
        }

        public async Task<string?> GetRefrashTokenAsync()
        {
            var tokens = await ReadAndDecryptTokensAsync();

            return tokens?.RefrashToken;
        }

        public async Task SaveAsync(string accessToken, string refrashToken)
        {
            var tokens = new Tokens(accessToken, refrashToken);
            var jsonString = JsonSerializer.Serialize(tokens);
            var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
            _entropy = GetOrCreateEntropy();
            var encryptedBytes = ProtectedData.Protect(jsonBytes, _entropy, DataProtectionScope.CurrentUser);

            await File.WriteAllBytesAsync(_tokenFilePath, encryptedBytes);
        }

        public Task ClearAsync()
        {
            if (File.Exists(_tokenFilePath))
                File.Delete(_tokenFilePath);

            return Task.CompletedTask;
        }

        private async Task<Tokens> ReadAndDecryptTokensAsync()
        {
            if (!File.Exists(_tokenFilePath))
                return null!;

            try
            {
                var encryptedBytes = await File.ReadAllBytesAsync(_tokenFilePath);
                var decryptedBytes = ProtectedData.Unprotect(encryptedBytes, _entropy, DataProtectionScope.CurrentUser);

                var jsonString = Encoding.UTF8.GetString(decryptedBytes);
                var tokens = JsonSerializer.Deserialize<Tokens>(jsonString);

                return tokens!;
            }
            catch (Exception)
            {
                return null!;
            }
        }

        private byte[] GetOrCreateEntropy()
        {
            if (_entropy != null)
                return _entropy;

            if (File.Exists(_entropyFilePath))
                return File.ReadAllBytes(_entropyFilePath);

            _entropy = RandomNumberGenerator.GetBytes(16);

            Directory.CreateDirectory(_filePath);

            File.WriteAllBytes(_entropyFilePath, _entropy);

            File.SetAttributes(_entropyFilePath, FileAttributes.Hidden);

            return _entropy;
        }
    }
}
