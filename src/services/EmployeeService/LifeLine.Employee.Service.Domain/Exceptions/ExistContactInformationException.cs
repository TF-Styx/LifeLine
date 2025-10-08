using Shared.Kernel.Exceptions;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class ExistContactInformationException(string message) : DomainException(message);
}
