using LifeLine.Directory.Service.Application.Features.Departments.Positions.Create;
using LifeLine.Directory.Service.Application.Features.Departments.Positions.Delete;
using LifeLine.Directory.Service.Application.Features.Departments.Positions.Get.GetAll;
using LifeLine.Directory.Service.Application.Features.Departments.Positions.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.DirectoryService.Position;

namespace LifeLine.Directory.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/departments/{departmentId}/positions")]
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
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] Guid departmentId, CancellationToken cancellationToken = default)
            => Ok(await _mediator.Send(new GetAllPositionByDepartmentQuery(departmentId), cancellationToken));

        [HttpPut("{positionId}")]
        public async Task<IActionResult> Update([FromRoute] Guid departmentId, [FromRoute] Guid positionId, [FromBody] UpdatePositionRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdatePositionCommand(departmentId, positionId, request.Name, request.Description);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpDelete("{positionId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid departmentId, [FromRoute] Guid positionId, CancellationToken cancellationToken = default)
        {
            var command = new DeletePositionCommand(departmentId, positionId);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное удаление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
