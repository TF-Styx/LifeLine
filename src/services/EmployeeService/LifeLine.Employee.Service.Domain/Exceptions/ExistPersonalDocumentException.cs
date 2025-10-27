using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class ExistPersonalDocumentException(string message) : DomainException(message);
}
