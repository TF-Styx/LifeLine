using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace Shared.Domain.ValueObjects
{
    public readonly record struct DocumentTypeId
    {
        public readonly Guid Value { get; }

        private DocumentTypeId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static DocumentTypeId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(DocumentTypeId)} был передан пустой Guid!"));

            return new DocumentTypeId(value);
        }

        public static DocumentTypeId New() => new(Guid.NewGuid());

        public static DocumentTypeId? Null => null;

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(DocumentTypeId value) => value.Value;
    }
}
