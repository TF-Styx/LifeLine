using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace Shared.Domain.ValueObjects
{
    public readonly record struct PositionId
    {
        public readonly Guid Value { get; }

        private PositionId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static PositionId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(PositionId)} был передан пустой Guid!"));

            return new PositionId(value);
        }

        public static PositionId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(PositionId positionId) => positionId.Value;
    }
}
