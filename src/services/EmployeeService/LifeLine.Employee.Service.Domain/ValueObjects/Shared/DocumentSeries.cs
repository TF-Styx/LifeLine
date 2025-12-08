using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Shared
{
    public sealed record DocumentSeries
    {
        public const int MAX_LENGTH = 50;
        public const int MIN_LENGTH = 1;

        public string Value { get; } = null!;

        private DocumentSeries(string value) { Value = value; }

        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static DocumentSeries Create(string value)
        {
            GuardException.Against.That(string.IsNullOrEmpty(value), () => new EmptyNameException($"В структуру {nameof(DocumentSeries)} был передано пустое поле!"));
            GuardException.Against.That(value.Length > MAX_LENGTH || value.Length < MIN_LENGTH, () => new LengthException($"Длина серии должна быть в диапазоне от {MAX_LENGTH} до {MIN_LENGTH}"));

            return new DocumentSeries(value);
        }

        public static DocumentSeries? Null => null;

        public override string ToString() => Value.ToString();

        public static implicit operator string(DocumentSeries value) => value.Value;
    }
}
