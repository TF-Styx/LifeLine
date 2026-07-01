using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace Shared.Domain.ValueObjects
{
    public readonly record struct HospitalId
    {
        public readonly Guid Value { get; }

        private HospitalId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static HospitalId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(HospitalId)} был передан пустой Guid!"));

            return new HospitalId(value);
        }

        public static HospitalId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(HospitalId value) => value.Value;
    }
}
