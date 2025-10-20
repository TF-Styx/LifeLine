using LifeLine.Employee.Service.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Shared
{
    public readonly record struct Hours
    {
        public TimeSpan Value { get; }

        private Hours(TimeSpan value) { Value = value; }

        /// <exception cref="InvalidHoursException"></exception>
        public static Hours Create(double value)
        {
            TimeSpan timeSpan = TimeSpan.FromHours(value);

            GuardException.Against.That(timeSpan.TotalHours <= 0, () => new InvalidHoursException($"Общее количество часов обучения, должно быть больше '0'!"));

            return new Hours(timeSpan);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator TimeSpan(Hours value) => value.Value;
    }
}
