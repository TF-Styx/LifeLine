using LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Create;
using LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdateAddress;
using LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdateCorporateEmail;
using LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdateCorporatePhone;
using LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdatePersonalEmail;
using LifeLine.Employee.Service.Application.Features.Employees.ContactInformation.Update.UpdatePersonalPhone;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.EmployeeService.ContactInformation;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/employees/{employeeId}/contact-informations")]
    public class ContactInformationController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] Guid employeeId, [FromBody] CreateContactInformationRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateContactInformationCommand(employeeId, request.PersonalPhone, request.CorporatePhone, request.PersonalEmail, request.CorporateEmail, new CreateAddressCommandData(request.PostalCode, request.Region, request.City, request.Street, request.Building, request.Apartment));

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => 
                    {
                        var error = errors.FirstOrDefault();

                        Func<IActionResult> func = error?.ErrorCode switch
                        {
                            ErrorCode.ExistContactInformation => () => StatusCode(StatusCodes.Status409Conflict, error.Message),

                            _ => () => BadRequest(errors)
                        };

                        return func.Invoke();
                    } 
                );
        }

        [HttpPatch("personal-phone")]
        public async Task<IActionResult> UpdatePersonalPhone([FromRoute] Guid employeeId, [FromQuery] string personalPhone, CancellationToken cancellationToken = default)
        {
            var command = new UpdatePersonalPhoneCommand(employeeId, personalPhone);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch("corporate-phone")]
        public async Task<IActionResult> UpdateCorporatePhone([FromRoute] Guid employeeId, [FromQuery] string corporatePhone, CancellationToken cancellationToken = default)
        {
            var command = new UpdateCorporatePhoneCommand(employeeId, corporatePhone);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch("personal-email")]
        public async Task<IActionResult> UpdatePersonalEmail([FromRoute] Guid employeeId, [FromQuery] string personalEmail, CancellationToken cancellationToken = default)
        {
            var command = new UpdatePersonalEmailCommand(employeeId, personalEmail);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch("corporate-email")]
        public async Task<IActionResult> UpdateCorporateEmail([FromRoute] Guid employeeId, [FromQuery] string corporateEmail, CancellationToken cancellationToken = default)
        {
            var command = new UpdateCorporateEmailCommand(employeeId, corporateEmail);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch("address")]
        public async Task<IActionResult> UpdateCorporateEmail([FromRoute] Guid employeeId, [FromBody] UpdateAddressRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateAddressCommand(employeeId, request.PostalCode, request.Region, request.City, request.Street, request.Building, request.Apartment);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
