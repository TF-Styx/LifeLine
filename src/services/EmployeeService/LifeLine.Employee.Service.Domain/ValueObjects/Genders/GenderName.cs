using Shared.Domain.Exceptions;
using Shared.Domain.Validations;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Genders
{
    public sealed record GenderName
    {
        public const int MAX_LENGTH = 50;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        private GenderName(string value) { Value = value; }

        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="LengthException"></exception>
        /// <exception cref="IncorrectStringException"></exception>
        public static GenderName Create(string value)
        {
            GuardException.Against.That(string.IsNullOrEmpty(value), () => new EmptyNameException($"В структуру {nameof(GenderName)} было передано пустое поле!"));
            GuardException.Against.That(value.Length > MAX_LENGTH || value.Length < MIN_LENGTH, () => new LengthException($"Длина имени гендера поля должна быть в диапазоне от {MAX_LENGTH} до {MIN_LENGTH}"));
            GuardException.Against.That(StringValidator.ContainsInvalidChars(value), () => new IncorrectStringException($"В имени гендера должен быть только русский или английский алфавит!"));

            return new GenderName(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator string(GenderName name) => name.Value;
    }
}
