using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    /// <summary>
    /// Выбрасывает исключение если Surname пустая
    /// </summary>
    /// <param name="message"></param>
    public sealed class EmptySurnameException(string message) : DomainException(message);
}
