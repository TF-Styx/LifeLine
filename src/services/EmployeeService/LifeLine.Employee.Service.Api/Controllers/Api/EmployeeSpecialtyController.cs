using LifeLine.Employee.Service.Application.Features.Employees.EmployeeSpecialties.Add;
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
    }
}
