using LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Add;
using LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Delete;
using LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.EmployeeService.EmployeeSpecialty;

namespace LifeLine.Employee.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/employees/{employeeId}/employee-specialties")]
    public class EmployeeSpecialtyController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] Guid employeeId, [FromBody] CreateEmployeeSpecialtyRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateEmployeeSpecialtyCommand(employeeId, request.SpecialtyId);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromRoute] Guid employeeId, [FromBody] UpdateEmployeeSpecialtyRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateEmployeeSpecialtyCommand
                (
                    employeeId,
                    Guid.Parse(request.SpecialtyIdOld),
                    Guid.Parse(request.SpecialtyIdNew)
                );

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpDelete("{specialtyId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid employeeId, [FromRoute] Guid specialtyId, CancellationToken cancellationToken = default)
        {
            var command = new DeleteEmployeeSpecialtyCommand(employeeId, specialtyId);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное удаление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
