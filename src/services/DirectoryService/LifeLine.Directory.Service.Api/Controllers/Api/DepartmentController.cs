using LifeLine.Directory.Service.Application.Features.Departments.Create;
using LifeLine.Directory.Service.Application.Features.Departments.Delete;
using LifeLine.Directory.Service.Application.Features.Departments.ForceDelete;
using LifeLine.Directory.Service.Application.Features.Departments.Get.GetAll;
using LifeLine.Directory.Service.Application.Features.Departments.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
            var command = new CreateDepartmentCommand
                (
                    request.Name,
                    request.Description,
                    [.. request.Positions.Select(position => new CreateDepartmentPositionCommandData(position.Name, position.Description))],
                    new CreateDepartmentAddressCommandData
                    (
                        request.Address.PostalCode, 
                        request.Address.Region, 
                        request.Address.City, 
                        request.Address.Street, 
                        request.Address.Building, 
                        request.Address.Apartment
                    )
                );

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default) 
            => Ok(await _mediator.Send(new GetAllDepartmentQuery(), cancellationToken));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateDepartmentRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateDepartmentCommand(id, request.Name, request.Description);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное удаление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteDepartmentCommand(id);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное удаление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpDelete("{id}/force-delete")]
        public async Task<IActionResult> ForceDelete([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var command = new ForceDeleteDepartmentCommand(id);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное удаление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
