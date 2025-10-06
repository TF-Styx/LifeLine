using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    /// <summary>
    /// Выбрасывается при пустом имени
    /// </summary>
    /// <param name="message"></param>
    public sealed class EmptyNameException(string message) : DomainException(message);
}
