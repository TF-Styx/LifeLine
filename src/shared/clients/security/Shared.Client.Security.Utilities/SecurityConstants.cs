using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;

namespace Shared.Client.Security.Utilities
{
    public static class SecurityConstants
    {
        public static readonly BigInteger N = BigInteger.Parse("00B3F8C9A7D2E4F1C5A6B9D0E3F2C1A4B8D7E6F5A2C3D4E5F6A7B8C9D0E1F2A3B4C5D6E7F8A9B0C1D2E3F4A5B6C7D8E9F0A1B2C3D4E5F6A7B8C9D0E1F2A3B4C5D6E7F8A9B0C1D2E3F4A5B6C7D8E9F0A1B2C3D4E5F6A7B8C9D0E1F2A3B4C5D6E7F8A9B0C1D2E3F4A5B6C7D8E9F0A1B2C3D4E5F6A7B8C9D0E1F2A3", NumberStyles.HexNumber);
        public static readonly BigInteger g = new(2);
        public static readonly BigInteger k = GetK();

        public const int NONCE_SIZE = 12;
        public const int TAG_SIZE = 16;
        public const int ITERATIONS = 600_000;
        public const int KEY_SIZE = 32;
        public const int MODULUS_SIZE = 384;

        private static BigInteger GetK()
        {
            using var sha = SHA256.Create();

            byte[] nBytes = BigIntegerHelper.ToFixedLength(N, MODULUS_SIZE);
            byte[] gBytes = BigIntegerHelper.ToFixedLength(g, MODULUS_SIZE);

            byte[] combined = new byte[768];

            Buffer.BlockCopy(nBytes, 0, combined, 0, MODULUS_SIZE);
            Buffer.BlockCopy(gBytes, 0, combined, MODULUS_SIZE, MODULUS_SIZE);

            var hash = sha.ComputeHash(combined);

            return new BigInteger(hash, isUnsigned: true, isBigEndian: true);
        }
    }
}
