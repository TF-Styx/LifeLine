using LifeLine.Directory.Service.Application.Features.EducationLevels.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeLine.Directory.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/education-levels")]
    public class EducationLevelController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] string educationLevelName, CancellationToken cancellationToken = default)
        {
            var command = new CreateEducationLevelCommand(educationLevelName);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
