using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.WorkPermits
{
    public readonly record struct WorkPermitId
    {
        public readonly Guid Value { get; }

        private WorkPermitId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static WorkPermitId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(WorkPermitId)} был передан пустой Guid!"));

            return new WorkPermitId(value);
        }

        public static WorkPermitId New() => new(Guid.NewGuid());

        public static WorkPermitId? Null => null;

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(WorkPermitId value) => value.Value;
    }
}
