using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    public sealed class AddressException(string message) : DomainException(message);
}
