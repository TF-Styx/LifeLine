using LifeLine.Directory.Service.Application.Features.AdmissionStatuses.Create;
using LifeLine.Directory.Service.Application.Features.AdmissionStatuses.Get.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeLine.Directory.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/admission-statuses")]
    public class AdmissionStatusController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] string admissionStatusName, CancellationToken cancellationToken = default)
        {
            var command = new CreateAdmissionStatusCommand(admissionStatusName);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
            => Ok(await _mediator.Send(new GetAllAdmissionStatusQuery(), cancellationToken));
    }
}
