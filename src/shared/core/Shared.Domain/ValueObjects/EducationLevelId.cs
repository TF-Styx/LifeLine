using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace Shared.Domain.ValueObjects
{
    public readonly record struct EducationLevelId
    {
        public readonly Guid Value { get; }

        private EducationLevelId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static EducationLevelId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(EducationLevelId)} был передан пустой Guid!"));

            return new EducationLevelId(value);
        }

        public static EducationLevelId New() => new(Guid.NewGuid());

        public static EducationLevelId? Null => null;

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(EducationLevelId value) => value.Value;
    }
}
