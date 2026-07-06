using LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Create;
using LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.CreateMany;
using LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Delete;
using LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Get.GetAllWorkPermitByEmployeeId;
using LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Extensions;
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
                    request.BucketName,
                    request.FileName,
                    request.PermitTypeId,
                    request.AdmissionStatusId
                );

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(result.Value),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpPost("many")]
        public async Task<IActionResult> CreateMany([FromRoute] Guid employeeId, CreateManyWorkPermitsRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateManyWorkPermitsCommand
                (
                    employeeId,
                    [.. request.WorkPermits.Select
                        (
                            x => new CreateManyDataWorkPermitsCommand
                                (
                                    x.WorkPermitName,
                                    x.DocumentSeries,
                                    x.WorkPermitNumber,
                                    x.ProtocolNumber,
                                    x.SpecialtyName,
                                    x.IssuingAuthority,
                                    DateTime.Parse(x.IssueDate),
                                    DateTime.Parse(x.ExpiryDate),
                                    x.BucketName,
                                    x.FileName,
                                    Guid.Parse(x.PermitTypeId),
                                    Guid.Parse(x.AdmissionStatusId)
                                )
                        )
                    ]
                );

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByEmployee([FromRoute] Guid employeeId, CancellationToken cancellationToken = default)
            => Ok(await _mediator.Send(new GetAllWorkPermitByEmployeeIdQuery(employeeId), cancellationToken));

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
                    request.BucketName,
                    request.FileName,
                    Guid.Parse(request.PermitTypeId),
                    Guid.Parse(request.AdmissionStatusId)
                );

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpDelete("{workPermitId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid employeeId, [FromRoute] Guid workPermitId, CancellationToken cancellationToken = default)
        {
            var command = new DeleteWorkPermitCommand(employeeId, workPermitId);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }
    }
}
