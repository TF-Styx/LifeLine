using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class ExistEducationDocumentException(string message) : DomainException(message);
}
