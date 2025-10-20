using LifeLine.Directory.Service.Application.Features.PermitTypes.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeLine.Directory.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/permit-types")]
    public class PermitTypeController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] string permitTypeName, CancellationToken cancellationToken = default)
        {
            var command = new CreatePermitTypeCommand(permitTypeName);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
