using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Employees
{
    public readonly record struct EmployeeId
    {
        public readonly Guid Value { get; }

        private EmployeeId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static EmployeeId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(EmployeeId)} был передан пустой Guid!"));

            return new EmployeeId(value);
        }

        public static EmployeeId New() => new(Guid.NewGuid());

        public static EmployeeId? Null => null;

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(EmployeeId employeeId) => employeeId.Value;
    }
}
