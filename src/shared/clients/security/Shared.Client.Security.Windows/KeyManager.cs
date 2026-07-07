using System.Text;
using System.Text.Json;
using Terminex.Common.Primitives;
using System.Security.Cryptography;
using Shared.Client.Security.Abstraction;

namespace Shared.Client.Security.Windows
{
    public sealed class KeyManager : IKeyManager
    {
        private static readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LifeLine", "dek.data");
        private static readonly byte[] s_entropy = Encoding.Unicode.GetBytes("L2MOoWYp1}!iiULj+#]|`YG>+~s3-%n~");

        public void SaveDek(byte[] value)
        {
            var jsonString = JsonSerializer.Serialize(value);
            var jsonBytes = Encoding.UTF8.GetBytes(jsonString);

            var protectedDate = ProtectedData.Protect(jsonBytes, s_entropy, DataProtectionScope.CurrentUser);

            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);

            File.WriteAllBytes(_filePath, protectedDate);
        }

        public Maybe<byte[]> GetDek()
        {
            if (File.Exists(_filePath))
                return null;

            try
            {
                var bytes = File.ReadAllBytes(_filePath);

                var unprotect = ProtectedData.Unprotect(bytes, s_entropy, DataProtectionScope.CurrentUser);

                var jsonString = Encoding.UTF8.GetString(unprotect);
                var key = JsonSerializer.Deserialize<byte[]>(jsonString);

                if (key == null || key.Length == 0)
                    return null;

                return key;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void ClearDek()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }

        public void Clear()
        {
            ClearDek();
        }
    }
}