using LifeLine.Directory.Service.Application.Features.Hospitals.Create;
using LifeLine.Directory.Service.Application.Features.Hospitals.Delete;
using LifeLine.Directory.Service.Application.Features.Hospitals.Get.GetAll;
using LifeLine.Directory.Service.Application.Features.Hospitals.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Extensions;
using Shared.Contracts.Request.DirectoryService.Hospital;

namespace LifeLine.Directory.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/hospitals")]
    public class HospitalController(IMediator mediator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHospitalRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateHospitalCommand
            (
                request.Name, 
                request.Description, 
                request.Phone, 
                request.Email, 
                new CreateHospitalDataAddressCommand
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
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
            => Ok(await mediator.Send(new GetAllHospitalQuery(), cancellationToken));

        [HttpPatch("{hospitalId}")]
        public async Task<IActionResult> Update([FromRoute] Guid hospitalId, [FromBody] UpdateHospitalRequest request, CancellationToken cancellation = default)
        {
            var command = new UpdateHospitalCommand
            (
                hospitalId,
                request.Name, 
                request.Description, 
                request.Phone, 
                request.Email, 
                new UpdateHospitalDataAddressCommand
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
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpDelete("{hospitalId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid hospitalId, CancellationToken cancellationToken = default)
        {
            var command = new DeleteHospitalCommand(hospitalId);

            var result = await mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }
    }
}
