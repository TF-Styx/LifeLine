using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Specialties
{
    public readonly record struct SpecialtyId
    {
        public readonly Guid Value { get; }

        private SpecialtyId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static SpecialtyId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(SpecialtyId)} был передан пустой Guid!"));

            return new SpecialtyId(value);
        }

        public static SpecialtyId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(SpecialtyId value) => value.Value;
    }
}
