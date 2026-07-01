using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Directory.Service.Domain.ValueObjects.AddressVO
{
    public sealed record Office
    {
        public const int MAX_LENGTH = 50;

        public string Value { get; } = null!;

        private Office(string value) { Value = value; }

        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static Office Create(string value)
        {
            GuardException.Against.That(string.IsNullOrEmpty(value), () => new AddressException($"В структуру {nameof(Office)} был передано пустое поле!"));
            GuardException.Against.That(value.Length > MAX_LENGTH, () => new LengthException($"Длина серии должна быть в диапазоне от {MAX_LENGTH}"));

            return new Office(value);
        }

        public static Office? Null => null;

        public override string ToString() => Value.ToString();

        public static implicit operator string(Office value) => value.Value;
    }
}
