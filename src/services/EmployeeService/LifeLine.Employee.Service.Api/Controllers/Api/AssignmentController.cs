using LifeLine.Employee.Service.Application.Features.Employees.Assignments.Create;
using LifeLine.Employee.Service.Application.Features.Employees.Assignments.CreateMany;
using LifeLine.Employee.Service.Application.Features.Employees.Assignments.Delete;
using LifeLine.Employee.Service.Application.Features.Employees.Assignments.Get.GetAllByEmployeeId;
using LifeLine.Employee.Service.Application.Features.Employees.Assignments.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Extensions;
using Shared.Contracts.Request.EmployeeService.Assignment;

namespace LifeLine.Employee.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/employees/{employeeId}/assignments")]
    [Authorize]
    public class AssignmentController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] Guid employeeId, [FromBody] CreateAssignmentRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateAssignmentCommand
                (
                    employeeId,
                    Guid.Parse(request.PositionId),
                    Guid.Parse(request.DepartmentId),
                    Guid.Parse(request.BranchId),
                    request.ManagerId != null ? Guid.Parse(request.ManagerId) : null,
                    request.HireDate,
                    request.TerminationDate,
                    Guid.Parse(request.StatusId),
                    new CreateAssignmentContractCommand
                    (
                        Guid.Parse(request.Contract.EmployeeTypeId),
                        request.Contract.ContractNumber,
                        request.Contract.StartDate,
                        request.Contract.EndDate,
                        request.Contract.Salary,
                        request.Contract.BucketName,
                        request.Contract.FileName
                    )
                );

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(result.Value),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpPost("batch")]
        public async Task<IActionResult> CreateMany([FromRoute] Guid employeeId, [FromBody] CreateManyAssignmentsReqeust reqeust, CancellationToken cancellationToken = default)
        {
            var command = new CreateManyAssignmentsCommand
                (
                    employeeId,
                    [.. reqeust.Assignments.Select
                        (
                            x => new CreateManyDataAssignmentsCommand
                                (
                                    Guid.Parse(x.PositionId),
                                    Guid.Parse(x.DepartmentId),
                                    Guid.Parse(x.BranchId),
                                    !string.IsNullOrWhiteSpace(x.ManagerId) ? Guid.Parse(x.ManagerId) : null,
                                    x.HireDate,
                                    x.TerminationDate,
                                    Guid.Parse(x.StatusId),
                                    new CreateManyDataAssignmentContractCommand
                                        (
                                            Guid.Parse(x.Contracts.EmployeeTypeId),
                                            x.Contracts.ContractNumber,
                                            x.Contracts.StartDate,
                                            x.Contracts.EndDate,
                                            x.Contracts.Salary,
                                            x.Contracts.BucketName,
                                            x.Contracts.FileName
                                        )
                                )
                        )                    
                    ]
                );

            var result = await _mediator.Send(command);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByEmployeeId([FromRoute] Guid employeeId, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(new GetAllAssignmentsContractsByEmployeeIdQuery(employeeId), cancellationToken));

        [HttpPatch("{assignmentId}/{contractId}")]
        public async Task<IActionResult> Update([FromRoute] Guid employeeId, [FromRoute] Guid assignmentId, [FromRoute] Guid contractId, UpdateAssignmentRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateAssignmentCommand
                (
                    assignmentId,
                    employeeId,
                    Guid.Parse(request.PositionId),
                    Guid.Parse(request.DepartmentId),
                    Guid.Parse(request.BranchId),
                    request.ManagerId != null ? Guid.Parse(request.ManagerId) : null,
                    request.HireDate,
                    request.TerminationDate,
                    Guid.Parse(request.StatusId),
                    new UpdateAssignmentContractCommand
                    (
                        contractId,
                        employeeId,
                        Guid.Parse(request.Contract.EmployeeTypeId),
                        request.Contract.ContractNumber,
                        request.Contract.StartDate,
                        request.Contract.EndDate,
                        request.Contract.Salary,
                        request.Contract.BucketName,
                        request.Contract.FileName
                    )
                );

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpDelete("{assignmentId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid employeeId, [FromRoute] Guid assignmentId, CancellationToken cancellationToken = default)
        {
            var command = new DeleteAssignmentCommand(employeeId, assignmentId);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }
    }
}
