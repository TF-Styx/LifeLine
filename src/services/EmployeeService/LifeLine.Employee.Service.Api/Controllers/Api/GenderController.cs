using LifeLine.Employee.Service.Application.Features.Genders.Create;
using LifeLine.Employee.Service.Application.Features.Genders.Delete;
using LifeLine.Employee.Service.Application.Features.Genders.Get.GetAll;
using LifeLine.Employee.Service.Application.Features.Genders.Get.GetById;
using LifeLine.Employee.Service.Application.Features.Genders.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.EmployeeService.GenderRequests;

namespace LifeLine.Employee.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/genders")]
    public class GenderController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGenderRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateGenderCommand(request.Name);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllGendersQuery(), cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id}/get-by-id")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetByIdGenderQuery(id), cancellationToken);

            return Ok(result.Value);
        }

        [HttpPatch("{id}/update-name")]
        public async Task<IActionResult> UpdateName([FromRoute] Guid id, [FromBody] UpdateGenderNameRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateGenderNameCommand(id, request.Name);

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
            var command = new DeleteGenderCommand(id);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное удаление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
