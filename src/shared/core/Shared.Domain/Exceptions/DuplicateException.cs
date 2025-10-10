using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    public sealed class DuplicateException(string message) : DomainException(message);
}
