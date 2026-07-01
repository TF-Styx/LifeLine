using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace Shared.Domain.ValueObjects
{
    public readonly record struct BranchId
    {
        public readonly Guid Value { get; }

        private BranchId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static BranchId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(BranchId)} был передан пустой Guid!"));

            return new BranchId(value);
        }

        public static BranchId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(BranchId value) => value.Value;
    }
}
