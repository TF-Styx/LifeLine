using LifeLine.Employee.Service.Application.Features.Specialties.Create;
using LifeLine.Employee.Service.Application.Features.Specialties.Get.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.EmployeeService.Specialty;

namespace LifeLine.Employee.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/specialties")]
    public class SpecialtyController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSpecialtyRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateSpecialtyCommand(request.SpecialtyName, request.Description);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
            => Ok(await _mediator.Send(new GetAllSpecialtyQuery(), cancellationToken));
    }
}
