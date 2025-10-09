using LifeLine.Directory.Service.Application.Features.Statuses.Create;
using LifeLine.Directory.Service.Application.Features.Statuses.Delete;
using LifeLine.Directory.Service.Application.Features.Statuses.Get.GetAll;
using LifeLine.Directory.Service.Application.Features.Statuses.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.DirectoryService.Status;

namespace LifeLine.Directory.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/statuses")]
    public class StatusController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStatusRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateStatusCommand(request.Name, request.Description);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
            => Ok(await _mediator.Send(new GetAllStatusQuery(), cancellationToken));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStatusRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateStatusCommand(id, request.Name, request.Description);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteStatusCommand(id);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное удаление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
