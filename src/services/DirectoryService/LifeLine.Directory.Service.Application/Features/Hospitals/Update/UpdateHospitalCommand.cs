using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Update
{
    public sealed record UpdateHospitalCommand
    (
        Guid Id, 
        string Name, 
        string? Description, 
        string Phone, 
        string Email, 
        UpdateHospitalDataAddressCommand Address
    ) : IRequest<Result<Nothing>>;

    public sealed record UpdateHospitalDataAddressCommand
    (
        string PostalCode, 
        string Region, 
        string City, 
        string Street
    );
}
