using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.PersonalDocuments
{
    public readonly record struct PersonalDocumentId
    {
        public readonly Guid Value { get; }

        private PersonalDocumentId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static PersonalDocumentId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(PersonalDocumentId)} был передан пустой Guid!"));

            return new PersonalDocumentId(value);
        }

        public static PersonalDocumentId New() => new(Guid.NewGuid());

        public static PersonalDocumentId? Null => null;

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(PersonalDocumentId value) => value.Value;
    }
}
