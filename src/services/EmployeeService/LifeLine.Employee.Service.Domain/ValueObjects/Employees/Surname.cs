using Shared.Domain.Exceptions;
using Shared.Domain.Validations;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Employees
{
    public sealed record Surname
    {
        public const int MAX_LENGTH = 50;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        private Surname(string value) { Value = value; }

        /// <exception cref="EmptySurnameException"></exception>
        /// <exception cref="LengthException"></exception>
        /// <exception cref="IncorrectStringException"></exception>
        public static Surname Create(string value)
        {
            GuardException.Against.That(string.IsNullOrEmpty(value), () => new EmptySurnameException($"В структуру {nameof(Surname)} был передано пустое поле!"));
            GuardException.Against.That(value.Length > MAX_LENGTH || value.Length < MIN_LENGTH, () => new LengthException($"Длина фамилии должна быть в диапазоне от {MAX_LENGTH} до {MIN_LENGTH}"));
            GuardException.Against.That(StringValidator.ContainsInvalidChars(value), () => new IncorrectStringException($"В фамилии должен быть только русский или английский алфавит!"));

            return new Surname(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator string(Surname surname) => surname.Value;
    }
}
