using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    /// <summary>
    /// Попытка установки того же значение
    /// </summary>
    /// <param name="message"></param>
    public sealed class IdenticalValuesException(string message) : DomainException(message);
}
