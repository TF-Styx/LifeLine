using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace Shared.Domain.ValueObjects
{
    public readonly record struct AdmissionStatusId
    {
        public readonly Guid Value { get; }

        private AdmissionStatusId(Guid value) { Value = value; }

        /// <exception cref="EmptyIdentifierException"></exception>
        public static AdmissionStatusId Create(Guid value)
        {
            GuardException.Against.That(value == Guid.Empty, () => new EmptyIdentifierException($"В структуру {nameof(AdmissionStatusId)} был передан пустой Guid!"));

            return new AdmissionStatusId(value);
        }

        public static AdmissionStatusId New() => new(Guid.NewGuid());

        public static AdmissionStatusId? Null => null;

        public override string ToString() => Value.ToString();

        public static implicit operator Guid(AdmissionStatusId value) => value.Value;
    }
}
