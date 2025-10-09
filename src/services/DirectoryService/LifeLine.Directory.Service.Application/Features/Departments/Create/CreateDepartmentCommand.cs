using MediatR;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Create
{
    public sealed record CreateDepartmentCommand
        (
            string Name, 
            string Description, 
            List<CreateDepartmentPositionCommandData> Positions, 
            CreateDepartmentAddressCommandData Address
        ) : IRequest<Result<Guid>>;

    public sealed record CreateDepartmentAddressCommandData(string PostalCode, string Region, string City, string Street, string Building, string? Apartment);

    public sealed record CreateDepartmentPositionCommandData(string Name, string Description);
}
