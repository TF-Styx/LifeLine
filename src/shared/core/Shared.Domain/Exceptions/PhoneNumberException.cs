using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    public sealed class PhoneNumberException(string message) : DomainException(message);
}
