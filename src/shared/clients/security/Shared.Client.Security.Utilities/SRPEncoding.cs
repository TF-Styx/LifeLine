using System.Numerics;

namespace Shared.Client.Security.Utilities
{
    public static class SRPEncoding
    {
        public static byte[] ToModulusBytes(BigInteger value) =>
            BigIntegerHelper.ToFixedLength(value, SecurityConstants.MODULUS_SIZE);

        public static byte[] ToHashBytes(BigInteger value) =>
            BigIntegerHelper.ToFixedLength(value, 32);

        public static BigInteger HashModuli(params BigInteger[] values) =>
            BigIntegerHelper.Hash(values.Select(ToModulusBytes).ToArray());

        public static BigInteger HashMixed(params BigInteger[] values)
        {
            var buffers = new List<byte[]>();

            for (int i = 0; i < values.Length; i++)
                buffers.Add(ToModulusBytes(values[i]));

            return BigIntegerHelper.Hash(buffers.ToArray());
        }

        public static BigInteger HashExplicit(params (BigInteger Value, bool IsModulus)[] args) =>
            BigIntegerHelper.Hash(
                args.Select(x => x.IsModulus ? ToModulusBytes(x.Value) : ToHashBytes(x.Value)).ToArray()
            );

        public static BigInteger ComputeM1(BigInteger A, BigInteger B, BigInteger S) =>
        BigIntegerHelper.Hash(
            ToModulusBytes(A),
            ToModulusBytes(B),
            ToModulusBytes(S)
        );

        public static BigInteger ComputeM2(BigInteger A, BigInteger M1, BigInteger S) =>
        BigIntegerHelper.Hash(
            ToModulusBytes(A),
            ToHashBytes(M1),
            ToModulusBytes(S)
        );
    }
}
