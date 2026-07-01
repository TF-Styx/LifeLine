using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Directory.Service.Domain.ValueObjects.AddressVO
{
    public sealed record Street
    {
        public const int MAX_LENGTH = 200;

        public string Value { get; } = null!;

        private Street(string value) { Value = value; }

        /// <exception cref="AddressException"></exception>
        /// <exception cref="LengthException"></exception>
        public static Street Create(string value)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(value), () => new AddressException("Наименование улица не может быть пустой!"));
            GuardException.Against.That(value.Length > MAX_LENGTH, () => new LengthException($"Макимальная длина наименования улицы не должна превышать `{MAX_LENGTH} символов`!"));

            return new Street(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator string(Street value) => value.Value;
    }
}