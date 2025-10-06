using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    /// <summary>
    /// Выбрасывает исключение при не корректной валидации строки
    /// </summary>
    /// <param name="message"></param>
    public sealed class IncorrectStringException(string message) : DomainException(message);
}
