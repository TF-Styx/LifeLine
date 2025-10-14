using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.EmployeeType
{
    public readonly record struct EmployeeTypeId
    {
        public readonly Guid Value { get; }

        private EmployeeTypeId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static EmployeeTypeId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(EmployeeTypeId)} был передан пустой Guid!"));

            return new EmployeeTypeId(value);
        }

        public static EmployeeTypeId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(EmployeeTypeId employeeId) => employeeId.Value;
    }
}
