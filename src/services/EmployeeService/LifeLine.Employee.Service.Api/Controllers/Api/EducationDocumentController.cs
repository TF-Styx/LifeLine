using LifeLine.Employee.Service.Api.Models.Request;
using LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LifeLine.Employee.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/employees/{employeeId}/education-documents")]
    public class EducationDocumentController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] Guid employeeId, [FromBody] CreateEducationDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateEducationDocumentCommand
                (
                    employeeId,
                    request.EducationLevelId,
                    request.DocumentTypeId,
                    request.DocumentNumber,
                    request.IssuedDate,
                    request.OrganizationName,
                    request.QualificationAwardedName,
                    request.SpecialtyName,
                    request.ProgramName,
                    request.TotalHours != null ? TimeSpan.FromHours(request.TotalHours.Value) : TimeSpan.Zero
                );

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
