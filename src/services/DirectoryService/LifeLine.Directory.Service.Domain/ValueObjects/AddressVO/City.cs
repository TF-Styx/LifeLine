using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Directory.Service.Domain.ValueObjects.AddressVO
{
    public sealed record City
    {
        public const int MAX_LENGTH = 100;

        public string Value { get; } = null!;

        private City(string value) { Value = value; }

        /// <exception cref="AddressException"></exception>
        /// <exception cref="LengthException"></exception>
        public static City Create(string value)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(value), () => new AddressException("Наименование города/населенного пункта не может быть пустым!"));
            GuardException.Against.That(value.Length > MAX_LENGTH, () => new LengthException($"Макимальная длина нименования города не должна превышать `{MAX_LENGTH} символов`!"));

            return new City(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator string(City value) => value.Value;
    }
}