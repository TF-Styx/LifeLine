using LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Create;
using LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.EmployeeService.WorkPermit;

namespace LifeLine.Employee.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/employees/{employeeId}/work-permits")]
    public class WorkPermitController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] Guid employeeId, [FromBody] CreateWorkPermitRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateWorkPermitCommand
                (
                    employeeId,
                    request.WorkPermitName,
                    request.DocumentSeries,
                    request.WorkPermitNumber,
                    request.ProtocolNumber,
                    request.SpecialtyName,
                    request.IssuingAuthority,
                    request.IssueDate,
                    request.ExpiryDate,
                    request.PermitTypeId,
                    request.AdmissionStatusId
                );

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch("{workPermitId}")]
        public async Task<IActionResult> UpdateWorkPermit([FromRoute] Guid employeeId, [FromRoute] Guid workPermitId, UpdateWorkPermitRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateWorkPermitCommand
                (
                    workPermitId,
                    employeeId,
                    request.WorkPermitName,
                    request.DocumentSeries,
                    request.WorkPermitNumber,
                    request.ProtocolNumber,
                    request.SpecialtyName,
                    request.IssuingAuthority,
                    request.IssueDate,
                    request.ExpiryDate,
                    Guid.Parse(request.PermitTypeId),
                    Guid.Parse(request.AdmissionStatusId)
                );

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
