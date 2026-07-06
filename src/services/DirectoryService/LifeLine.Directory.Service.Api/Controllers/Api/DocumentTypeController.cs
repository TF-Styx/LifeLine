using LifeLine.Directory.Service.Application.Features.DocumentTypes.Create;
using LifeLine.Directory.Service.Application.Features.DocumentTypes.Get.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Api.Extensions;

namespace LifeLine.Directory.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/document-types")]
    public class DocumentTypeController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] string documentTypeName, CancellationToken cancellationToken = default)
        {
            var command = new CreateDocumentTypeCommand(documentTypeName);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok(result.Value),
                    onFailure: errors => this.MapActionResult(errors)
                );
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
            => Ok(await _mediator.Send(new GetAllDocumentTypeQuery(), cancellationToken));
    }
}
