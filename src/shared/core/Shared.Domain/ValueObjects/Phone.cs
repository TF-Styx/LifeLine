using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;
using System.Text.RegularExpressions;

namespace Shared.Domain.ValueObjects
{
    public sealed record Phone
    {
        public string Value { get; }

        public Phone(string value) => Value = value;

        private static readonly Regex PhoneRegex = new(@"^+?[0-9\s-]{7,20}$");

        /// <exception cref="PhoneNumberException"></exception>
        public static Phone Create(string phoneNumber)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(phoneNumber), () => new PhoneNumberException("Номер телефона не может быть пустым!"));

            var sanitizedPhone = phoneNumber.Trim();

            GuardException.Against.That(!PhoneRegex.IsMatch(sanitizedPhone), () => new PhoneNumberException("Неверный формат номера телефона!"));

            return new Phone(sanitizedPhone);
        }

        public static Phone? Null => null;

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value))
                return string.Empty;

            return Value;
        }

        public static implicit operator string(Phone phone) => phone.Value;
    }
}
