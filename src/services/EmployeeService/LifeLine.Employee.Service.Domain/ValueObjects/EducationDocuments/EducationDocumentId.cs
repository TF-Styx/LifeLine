using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.EducationDocuments
{
    public readonly record struct EducationDocumentId
    {
        public readonly Guid Value { get; }

        private EducationDocumentId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static EducationDocumentId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(EducationDocumentId)} был передан пустой Guid!"));

            return new EducationDocumentId(value);
        }

        public static EducationDocumentId New() => new(Guid.NewGuid());

        public static EducationDocumentId? Null => null;

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(EducationDocumentId value) => value.Value;
    }
}
