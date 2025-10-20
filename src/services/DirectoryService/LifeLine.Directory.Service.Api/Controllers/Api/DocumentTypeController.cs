using LifeLine.Directory.Service.Application.Features.DocumentTypes.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
