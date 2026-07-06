using LifeLine.Directory.Service.Application.Features.Departments.Create;
using LifeLine.Directory.Service.Application.Features.Departments.Delete;
using LifeLine.Directory.Service.Application.Features.Departments.Get.GetAll;
using LifeLine.Directory.Service.Application.Features.Departments.Get.GetAllByBranchId;
using LifeLine.Directory.Service.Application.Features.Departments.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Extensions;
using Shared.Contracts.Request.DirectoryService.Department;

namespace LifeLine.Directory.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/departments")]
    public class DepartmentController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateDepartmentCommand(request.Name, request.Description, request.Building, Guid.Parse(request.BranchId));

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(result.Value),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default) 
            => Ok(await _mediator.Send(new GetAllDepartmentQuery(), cancellationToken));

        [HttpGet("branch/{branchId}")]
        public async Task<IActionResult> GetAllByBranchId([FromRoute] Guid branchId, CancellationToken cancellationToken = default)
            => Ok(await _mediator.Send(new GetAllByBranchIdQuery(branchId), cancellationToken));

        [HttpPatch("{departmentId}")]
        public async Task<IActionResult> Update([FromRoute] Guid departmentId, [FromBody] UpdateDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateDepartmentCommand(departmentId, request.Name, request.Description, request.Building, Guid.Parse(request.BranchId));

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное удаление!"),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpDelete("{departmentId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid departmentId, CancellationToken cancellationToken = default)
        {
            var command = new DeleteDepartmentCommand(departmentId);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }
    }
}
