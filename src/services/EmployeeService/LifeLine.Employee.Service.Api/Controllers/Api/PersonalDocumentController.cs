using LifeLine.Employee.Service.Api.Models.Request;
using LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeLine.Employee.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/employees/{employeeId}/personal-documents")]
    public class PersonalDocumentController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] Guid employeeId, [FromBody] CreatePersonalDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreatePersonalDocumentCommand(employeeId, request.DocumentTypeId, request.DocumentNumber, request.DocumentSeries, null);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
