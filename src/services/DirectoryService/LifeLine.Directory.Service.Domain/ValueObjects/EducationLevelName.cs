using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Directory.Service.Domain.ValueObjects
{
    public sealed record EducationLevelName
    {
        public const int MAX_LENGTH = 100;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        private EducationLevelName(string value) { Value = value; }

        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static EducationLevelName Create(string value)
        {
            GuardException.Against.That(string.IsNullOrEmpty(value), () => new EmptyNameException($"В структуру {nameof(EducationLevelName)} был передано пустое поле!"));
            GuardException.Against.That(value.Length > MAX_LENGTH || value.Length < MIN_LENGTH, () => new LengthException($"Длина имени должна быть в диапазоне от {MAX_LENGTH} до {MIN_LENGTH}"));
            //GuardException.Against.That(StringValidator.ContainsInvalidChars(value), () => new IncorrectStringException($"В имени должен быть только русский или английский алфавит!"));

            return new EducationLevelName(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator string(EducationLevelName name) => name.Value;
    }
}
