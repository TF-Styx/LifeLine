using LifeLine.Employee.Service.Application.Features.EmployeeTypes.Create;
using LifeLine.Employee.Service.Application.Features.EmployeeTypes.Delete;
using LifeLine.Employee.Service.Application.Features.EmployeeTypes.Get.GetAll;
using LifeLine.Employee.Service.Application.Features.EmployeeTypes.Get.GetById;
using LifeLine.Employee.Service.Application.Features.EmployeeTypes.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Extensions;
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
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default) 
            => Ok(await _mediator.Send(new GetAllEmployeeTypeQuery(), cancellationToken));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken = default) 
            => Ok(await _mediator.Send(new GetByIdEmployeeTypeQuery(id), cancellationToken));

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateEmployeeTypeRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateEmployeeTypeCommand(id, request.Name, request.Description);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteEmployeeTypeCommand(id);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }
    }
}
