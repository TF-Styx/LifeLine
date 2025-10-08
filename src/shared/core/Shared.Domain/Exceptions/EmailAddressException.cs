using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    public sealed class EmailAddressException(string message) : DomainException(message);
}
