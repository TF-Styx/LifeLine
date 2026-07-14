using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Create
{
    public sealed record CreateHospitalCommand(string Name, string? Description, string Phone, string Email, CreateHospitalDataAddressCommand Address) : IRequest<Result<string>>;

    public sealed record CreateHospitalDataAddressCommand(string PostalCode, string Region, string City, string Street, string? Building, string? Apartment);
}
