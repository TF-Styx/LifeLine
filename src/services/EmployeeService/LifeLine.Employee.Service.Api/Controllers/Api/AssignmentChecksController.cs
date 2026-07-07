using LifeLine.Employee.Service.Application.Features.Employees.Assignments.HasActive.HasActiveDepartment;
using LifeLine.Employee.Service.Application.Features.Employees.Assignments.HasActive.HasActivePosition;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Extensions;

namespace LifeLine.Employee.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/assignments/checks")]
    [Authorize]
    public class AssignmentChecksController(IMediator mediator) : Controller
    {
        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> HasActiveAssignmentsToDepartment([FromRoute] Guid departmentId, CancellationToken cancellationToken = default)
        {
            var query = new GetDepartmentAssignmentsStatusQuery(departmentId);

            var result = await mediator.Send(query, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpGet("position/{positionId}")]
        public async Task<IActionResult> HasActiveAssignmentsToPosition([FromRoute] Guid positionId, CancellationToken cancellationToken = default)
        {
            var query = new GetPositionAssignmentsStatusQuery(positionId);

            var result = await mediator.Send(query, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }
    }
}
