using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class NotFoundSpecialtyException(string message) : DomainException(message);
}
