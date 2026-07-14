using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Branches.Create
{
    public sealed record CreateBranchCommand
    (
        string Name, 
        string? Description, 
        string Phone, 
        string Email, 
        Guid HospitalId, 
        CreateBranchDataAddressCommand Address
    ) : IRequest<Result<string>>;

    public sealed record CreateBranchDataAddressCommand
    (
        string PostalCode, 
        string Region, 
        string City, 
        string Street,
        string? Building,
        string? Apartment
    );
}
