using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class InvalidHoursException(string message) : DomainException(message);
}
