using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class EmptyPersonalDocumentException(string message) : DomainException(message);
}
