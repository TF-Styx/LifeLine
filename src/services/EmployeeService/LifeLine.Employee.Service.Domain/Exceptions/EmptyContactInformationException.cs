using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class EmptyContactInformationException(string message) : DomainException(message);
}
