using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class EmptyContractException(string message) : DomainException(message);
}
