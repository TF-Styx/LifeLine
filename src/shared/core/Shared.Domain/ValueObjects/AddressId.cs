using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace Shared.Domain.ValueObjects
{
    public readonly record struct AddressId
    {
        public readonly Guid Value { get; }

        private AddressId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static AddressId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(AddressId)} был передан пустой Guid!"));

            return new AddressId(value);
        }

        public static AddressId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(AddressId departmentId) => departmentId.Value;
    }
}
