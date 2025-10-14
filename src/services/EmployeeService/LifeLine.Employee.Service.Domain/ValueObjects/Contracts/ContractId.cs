using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Contracts
{
    public readonly record struct ContractId
    {
        public readonly Guid Value { get; }

        private ContractId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static ContractId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(ContractId)} был передан пустой Guid!"));

            return new ContractId(value);
        }

        public static ContractId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(ContractId contractId) => contractId.Value;
    }
}
