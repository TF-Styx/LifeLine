using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class EmptyAssignmentException(string message) : DomainException(message);
}
