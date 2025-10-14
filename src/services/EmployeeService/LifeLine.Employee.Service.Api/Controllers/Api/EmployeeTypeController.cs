using LifeLine.Employee.Service.Application.Features.EmployeeTypes.Create;
using LifeLine.Employee.Service.Application.Features.EmployeeTypes.Delete;
using LifeLine.Employee.Service.Application.Features.EmployeeTypes.Get.GetAll;
using LifeLine.Employee.Service.Application.Features.EmployeeTypes.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.EmployeeService.EmployeeType;

namespace LifeLine.Employee.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/employee-types")]
    public class EmployeeTypeController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeTypeRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateEmployeeTypeCommand(request.Name, request.Description);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default) 
            => Ok(await _mediator.Send(new GetAllEmployeeTypeQuery(), cancellationToken));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateEmployeeTypeRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateEmployeeTypeCommand(id, request.Name, request.Description);

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
            var command = new DeleteEmployeeTypeCommand(id);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное удаление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
