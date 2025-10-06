using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    /// <summary>
    /// Выбрасывается при пустом отчестве
    /// </summary>
    /// <param name="message"></param>
    public sealed class EmptyPatronymicException(string message) : DomainException(message);
}
