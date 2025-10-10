using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    public sealed class RecordMissingException(string message) : DomainException(message);
}
