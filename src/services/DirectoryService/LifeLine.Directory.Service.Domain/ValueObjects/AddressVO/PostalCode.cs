using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;
using System.Text.RegularExpressions;

namespace LifeLine.Directory.Service.Domain.ValueObjects.AddressVO
{
    public sealed record PostalCode
    {
        public const int MAX_LENGTH = 6;

        public string Value { get; } = null!;

        private PostalCode(string value) { Value = value; }

        /// <exception cref="AddressException"></exception>
        /// <exception cref="LengthException"></exception>
        public static PostalCode Create(string value)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(value), () => new AddressException("Почтовый индекс не может быть пустым!"));
            GuardException.Against.That(!Regex.IsMatch(value, @"^\d{6}$"), () => new AddressException("Неверный формат почтового индекса (требуется 6 цифр)!"));
            GuardException.Against.That(value.Length > MAX_LENGTH, () => new LengthException($"Длина почтового индекса должна быть: {MAX_LENGTH}"));

            return new PostalCode(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator string(PostalCode value) => value.Value;
    }
}