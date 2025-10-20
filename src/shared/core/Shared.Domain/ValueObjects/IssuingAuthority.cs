using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace Shared.Domain.ValueObjects
{
    public sealed record IssuingAuthority
    {
        public const int MAX_LENGTH = 500;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        private IssuingAuthority(string value) { Value = value; }

        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static IssuingAuthority Create(string value)
        {
            GuardException.Against.That(string.IsNullOrEmpty(value), () => new EmptyNameException($"В структуру {nameof(IssuingAuthority)} был передано пустое поле!"));
            GuardException.Against.That(value.Length > MAX_LENGTH || value.Length < MIN_LENGTH, () => new LengthException($"Длина кем выдан должна быть в диапазоне от {MAX_LENGTH} до {MIN_LENGTH}"));

            return new IssuingAuthority(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator string(IssuingAuthority value) => value.Value;
    }
}
