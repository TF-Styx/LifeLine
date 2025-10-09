using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace Shared.Domain.ValueObjects
{
    public readonly record struct StatusId
    {
        public readonly Guid Value { get; }

        private StatusId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static StatusId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(StatusId)} был передан пустой Guid!"));

            return new StatusId(value);
        }

        public static StatusId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(StatusId statusId) => statusId.Value;
    }
}
