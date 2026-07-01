using LifeLine.Directory.Service.Application.Features.Branches.Create;
using LifeLine.Directory.Service.Application.Features.Branches.Delete;
using LifeLine.Directory.Service.Application.Features.Branches.Get.GetAll;
using LifeLine.Directory.Service.Application.Features.Branches.Get.GetAllByHospitalId;
using LifeLine.Directory.Service.Application.Features.Branches.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.DirectoryService.Branch;

namespace LifeLine.Directory.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/branches")]
    public class BranchController(IMediator mediator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBranchRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateBranchCommand
            (
                request.Name,
                request.Description,
                request.Phone,
                request.Email,
                Guid.Parse(request.HospitalId),
                new CreateBranchDataAddressCommand
                (
                    request.Address.PostalCode,
                    request.Address.Region,
                    request.Address.City,
                    request.Address.Street
                )
            );

            var result = await mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(result.Value),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
            => Ok(await mediator.Send(new GetAllBranchQuery(), cancellationToken));

        [HttpGet("by-hospital-id/{hospitalId}")]
        public async Task<IActionResult> GetAllByHospitalId([FromRoute] Guid hospitalId, CancellationToken cancellationToken = default)
            => Ok(await mediator.Send(new GetAllByHospitalIdQuery(hospitalId), cancellationToken));

        [HttpPatch("{branchId}")]
        public async Task<IActionResult> Update([FromRoute] Guid branchId, [FromBody] UpdateBranchRequest request, CancellationToken cancellation = default)
        {
            var command = new UpdateBranchCommand
            (
                branchId,
                request.Name,
                request.Description,
                request.Phone,
                request.Email,
                Guid.Parse(request.HospitalId),
                new UpdateBranchDataAddressCommand
                (
                    request.Address.PostalCode,
                    request.Address.Region,
                    request.Address.City,
                    request.Address.Street
                )
            );

            var result = await mediator.Send(command, cancellation);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpDelete("{branchId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid branchId, CancellationToken cancellationToken = default)
        {
            var command = new DeleteBranchCommand(branchId);

            var result = await mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
