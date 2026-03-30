using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class ExistWorkPermitException(string message) : DomainException(message);
}
