using Shared.Domain.Exceptions;
using Shared.Domain.Validations;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Employees
{
    public sealed record Patronymic
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        private Patronymic(string value) { Value = value; }

        /// <exception cref="EmptyPatronymicException"></exception>
        /// <exception cref="LengthException"></exception>
        /// <exception cref="IncorrectStringException"></exception>
        public static Patronymic Create(string value)
        {
            GuardException.Against.That(!string.IsNullOrEmpty(value), () => new EmptyPatronymicException($"В структуру {nameof(Patronymic)} был передано пустое поле!"));
            GuardException.Against.That(value.Length > MAX_LENGTH || value.Length < MIN_LENGTH, () => new LengthException($"Длина отчества должна быть в диапазоне от {MAX_LENGTH} до {MIN_LENGTH}"));
            GuardException.Against.That(StringValidator.ContainsInvalidChars(value), () => new IncorrectStringException($"В отчестве должен быть только русский или английский алфавит!"));

            return new Patronymic(value);
        }

        public static Patronymic? Empty => null;

        public override string ToString() => Value.ToString();

        public static implicit operator string(Patronymic patronymic) => patronymic.Value;
    }
}
