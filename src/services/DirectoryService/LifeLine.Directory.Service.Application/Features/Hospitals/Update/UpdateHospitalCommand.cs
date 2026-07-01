using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Hospitals.Update
{
    public sealed record UpdateHospitalCommand(Guid Id, string Name, string? Description, string Phone, string Email, UpdateHospitalDataAddressCommand Address) : IRequest<Result>;

    public sealed record UpdateHospitalDataAddressCommand(string PostalCode, string Region, string City, string Street);
}
