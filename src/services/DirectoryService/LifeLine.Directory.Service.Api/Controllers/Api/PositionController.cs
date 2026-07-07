using LifeLine.Directory.Service.Application.Features.Departments.Positions.Create;
using LifeLine.Directory.Service.Application.Features.Departments.Positions.Delete;
using LifeLine.Directory.Service.Application.Features.Departments.Positions.Get.GetAllByDepartmentId;
using LifeLine.Directory.Service.Application.Features.Departments.Positions.Get.GetAllPosition;
using LifeLine.Directory.Service.Application.Features.Departments.Positions.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Extensions;
using Shared.Contracts.Request.DirectoryService.Position;

namespace LifeLine.Directory.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/departments/{departmentId}/positions")]
    [Authorize]
    public class PositionController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] Guid departmentId, CreatePositionRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreatePositionCommand(departmentId, request.Name, request.Description);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(result.Value),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] Guid departmentId, CancellationToken cancellationToken = default)
            => Ok(await _mediator.Send(new GetAllByDepartmentIdQuery(departmentId), cancellationToken));

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPosition(CancellationToken cancellationToken = default)
            => Ok(await _mediator.Send(new GetAllPositionQuery(), cancellationToken));

        [HttpPut("{positionId}")]
        public async Task<IActionResult> Update([FromRoute] Guid departmentId, [FromRoute] Guid positionId, [FromBody] UpdatePositionRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdatePositionCommand(departmentId, positionId, request.Name, request.Description);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpDelete("{positionId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid departmentId, [FromRoute] Guid positionId, CancellationToken cancellationToken = default)
        {
            var command = new DeletePositionCommand(departmentId, positionId);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }
    }
}
