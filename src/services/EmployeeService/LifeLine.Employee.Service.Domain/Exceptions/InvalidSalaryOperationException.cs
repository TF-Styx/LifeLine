using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class InvalidSalaryOperationException(string message) : DomainException(message);
}
