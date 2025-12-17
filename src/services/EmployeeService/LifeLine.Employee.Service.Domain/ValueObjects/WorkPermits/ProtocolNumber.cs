using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.WorkPermits
{
    public sealed record ProtocolNumber
    {
        public const int MAX_LENGTH = 150;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        private ProtocolNumber(string value) { Value = value; }

        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static ProtocolNumber Create(string value)
        {
            GuardException.Against.That(string.IsNullOrEmpty(value), () => new EmptyNameException($"В структуру {nameof(ProtocolNumber)} был передано пустое поле!"));
            GuardException.Against.That(value.Length > MAX_LENGTH || value.Length < MIN_LENGTH, () => new LengthException($"Длина номера должна быть в диапазоне от {MAX_LENGTH} до {MIN_LENGTH}"));

            return new ProtocolNumber(value);
        }

        public static ProtocolNumber? Null => null;

        public override string ToString() => Value.ToString();

        public static implicit operator string(ProtocolNumber value) => value.Value;
    }
}
