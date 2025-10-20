using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace Shared.Domain.ValueObjects
{
    public readonly record struct PermitTypeId
    {
        public readonly Guid Value { get; }

        private PermitTypeId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static PermitTypeId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(PermitTypeId)} был передан пустой Guid!"));

            return new PermitTypeId(value);
        }

        public static PermitTypeId New() => new(Guid.NewGuid());

        public static PermitTypeId? Null => null;

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(PermitTypeId value) => value.Value;
    }
}
