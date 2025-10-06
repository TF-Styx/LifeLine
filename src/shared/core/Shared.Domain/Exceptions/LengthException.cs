using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    /// <summary>
    /// Выбрасывает исключение если у поля превышена или недостаточна длина
    /// </summary>
    /// <param name="message"></param>
    public class LengthException(string message) : DomainException(message);
}
