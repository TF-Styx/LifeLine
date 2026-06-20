using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Update
{
    public sealed record UpdateDepartmentCommand(Guid Id, string Name, string Description, UpdateDepartmentDataAddressCommand Address) : IRequest<Result>;

    public sealed record UpdateDepartmentDataAddressCommand(string PostalCode, string Region, string City, string Street, string Building);
}
