using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class EmptyEmployeeSpecialtyException(string message) : DomainException(message);
}
