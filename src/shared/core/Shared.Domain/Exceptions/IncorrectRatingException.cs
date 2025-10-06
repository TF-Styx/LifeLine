using Shared.Kernel.Exceptions;

namespace Shared.Domain.Exceptions
{
    /// <summary>
    /// Выбрасывается исключение если не корректный рейтинг
    /// </summary>
    /// <param name="message"></param>
    public sealed class IncorrectRatingException(string message) : DomainException(message);
}
