using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Directory.Service.Domain.ValueObjects.AddressVO
{
    public sealed record Building
    {
        public const int MAX_LENGTH = 50;

        public string Value { get; } = null!;

        private Building(string value) { Value = value; }

        /// <exception cref="AddressException"></exception>
        /// <exception cref="LengthException"></exception>
        public static Building Create(string value)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(value), () => new AddressException("Номер дома не может быть пустым!"));
            GuardException.Against.That(value.Length > MAX_LENGTH, () => new LengthException($"Макимальная длина обозначения дома не должна превышать `{MAX_LENGTH} символов`!"));

            return new Building(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator string(Building value) => value.Value;
    }
}