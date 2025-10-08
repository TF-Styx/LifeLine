using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.ContactInformation
{
    public readonly record struct ContactInformationId
    {
        public Guid Value { get; }

        private ContactInformationId(Guid value) => Value = value;

        /// <exception cref="EmptyIdentifierException"></exception>
        public static ContactInformationId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(ContactInformationId)} был передан пустой Guid!"));

            return new ContactInformationId(value);
        }

        public static ContactInformationId New() => new(Guid.NewGuid());

        public static implicit operator Guid(ContactInformationId value) => value.Value;
        public override string ToString() => Value.ToString();
    }
}
