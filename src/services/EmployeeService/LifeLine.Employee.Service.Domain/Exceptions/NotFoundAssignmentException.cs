using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class NotFoundAssignmentException(string message) : DomainException(message);
}
