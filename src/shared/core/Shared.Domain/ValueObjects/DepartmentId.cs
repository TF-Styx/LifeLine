using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace Shared.Domain.ValueObjects
{
    public readonly record struct DepartmentId
    {
        public readonly Guid Value { get; }

        private DepartmentId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static DepartmentId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(DepartmentId)} был передан пустой Guid!"));

            return new DepartmentId(value);
        }

        public static DepartmentId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(DepartmentId departmentId) => departmentId.Value;
    }
}
