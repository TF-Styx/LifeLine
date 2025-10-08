using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;
using System.Text.RegularExpressions;

namespace Shared.Domain.ValueObjects
{
    public sealed record Email
    {
        public string Value { get; } = null!;

        internal Email(string value) => Value = value;

        /// <exception cref="EmailAddressException"></exception>
        public static Email Create(string email)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(email), () => new EmailAddressException("Email не может быть пустым!"));
            GuardException.Against.That(!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+.[^@\s]+$"), () => new EmailAddressException("Неверный формат Email!"));

            return new Email(email);
        }

        public static Email? Null => null;

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Value))
                return string.Empty;

            return Value;
        }

        public static implicit operator string(Email email) => email.Value;
    }
}
