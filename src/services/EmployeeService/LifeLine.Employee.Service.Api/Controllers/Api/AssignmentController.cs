using LifeLine.Employee.Service.Api.Models.Request;
using LifeLine.Employee.Service.Application.Features.Assignments.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeLine.Employee.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/employees/{employeeId}/assignments")]
    public class AssignmentController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] Guid employeeId, [FromBody] CreateAssignmentRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateAssignmentCommand
                (
                    employeeId, 
                    request.PositionId, 
                    request.DepartmentId, 
                    request.ManagerId, 
                    request.HireDate, 
                    request.TerminationDate, 
                    request.StatusId, 
                    new CreateAssignmentContractCommand
                    (
                        request.Contract.EmployeeTypeId, 
                        request.Contract.ContractNumber, 
                        request.Contract.StartDate, 
                        request.Contract.EndDate, 
                        request.Contract.Salary, 
                        null
                    )
                );

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
