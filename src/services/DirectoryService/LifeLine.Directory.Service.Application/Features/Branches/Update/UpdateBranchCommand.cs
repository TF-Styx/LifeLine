using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.Branches.Update
{
    public sealed record UpdateBranchCommand(Guid BranchId, string Name, string? Description, string Phone, string Email, Guid HospitalId, UpdateBranchDataAddressCommand Address) : IRequest<Result>;

    public sealed record UpdateBranchDataAddressCommand(string PostalCode, string Region, string City, string Street);
}
