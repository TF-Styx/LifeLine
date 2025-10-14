using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Assignments
{
    public readonly record struct AssignmentId
    {
        public readonly Guid Value { get; }

        private AssignmentId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static AssignmentId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(AssignmentId)} был передан пустой Guid!"));

            return new AssignmentId(value);
        }

        public static AssignmentId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(AssignmentId assignmentId) => assignmentId.Value;
    }
}
