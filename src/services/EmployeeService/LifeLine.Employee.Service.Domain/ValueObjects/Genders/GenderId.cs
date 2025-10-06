using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Genders
{
    public readonly record struct GenderId
    {
        public readonly Guid Value { get; }

        private GenderId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static GenderId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(GenderId)} был передан пустой Guid!"));

            return new GenderId(value);
        }

        public static GenderId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(GenderId employeeId) => employeeId.Value;
    }
}
