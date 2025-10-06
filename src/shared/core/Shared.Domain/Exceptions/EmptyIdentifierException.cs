using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    /// <summary>
    /// Выбрасывается если Id пустое
    /// </summary>
    /// <param name="message"></param>
    public sealed class EmptyIdentifierException(string message) : DomainException(message);
}
