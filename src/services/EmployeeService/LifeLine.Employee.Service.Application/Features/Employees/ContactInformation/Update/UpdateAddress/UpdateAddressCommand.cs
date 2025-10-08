using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdateAddress
{
    public sealed record UpdateAddressCommand(Guid EmployeeId, string PostalCode, string Region, string City, string Street, string Building, string? Apartment) : IRequest<Result>;
}
