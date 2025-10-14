using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class SalaryOutOfRangeException(string message) : DomainException(message);
}
