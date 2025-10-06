using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Employees
{
    public readonly record struct Rating
    {
        public const double MAX_VALUE = 5.0;
        public const double MIN_VALUE = 0.0;

        public double Value { get; }

        private Rating(double value) => Value = value;
        
        public static Rating Create(double value)
        {
            GuardException.Against.That(value > MAX_VALUE || value < MIN_VALUE, () => new IncorrectRatingException($"Рейтинг должен быть в диапазоне от {MAX_VALUE} до {MIN_VALUE}"));

            return new Rating(value);
        }

        public static Rating DefaultRating => new(0.0);

        public override string ToString() => Value.ToString();

        public static implicit operator double(Rating rating) => rating.Value;
    }
}
