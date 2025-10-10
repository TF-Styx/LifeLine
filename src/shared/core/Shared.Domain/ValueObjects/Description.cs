using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace Shared.Domain.ValueObjects
{
    public sealed record Description
    {
        public const int MAX_LENGTH = 5000;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        private Description(string value) { Value = value; }

        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static Description Create(string value)
        {
            GuardException.Against.That(string.IsNullOrEmpty(value), () => new EmptyNameException($"В структуру {nameof(Description)} был передано пустое поле!"));
            GuardException.Against.That(value.Length > MAX_LENGTH || value.Length < MIN_LENGTH, () => new LengthException($"Длина описания должна быть в диапазоне от {MAX_LENGTH} до {MIN_LENGTH}"));
            //GuardException.Against.That(StringValidator.ContainsInvalidChars(value), () => new IncorrectStringException($"В имени должен быть только русский или английский алфавит!"));

            return new Description(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator string(Description name) => name.Value;
    }
}
