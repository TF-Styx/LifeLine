using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Create
{
    public sealed record CreateDepartmentCommand
        (
            string Name, 
            string Description, 
            CreateDepartmentAddressCommandData Address
        ) : IRequest<Result<string>>;

    public sealed record CreateDepartmentAddressCommandData(string PostalCode, string Region, string City, string Street, string Building);
}
