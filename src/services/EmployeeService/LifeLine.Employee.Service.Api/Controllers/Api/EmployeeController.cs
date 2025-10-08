﻿using LifeLine.Employee.Service.Application.Features.Employees.Create;
using LifeLine.Employee.Service.Application.Features.Employees.Delete;
using LifeLine.Employee.Service.Application.Features.Employees.Get.GetAll;
using LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployee;
using LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeeGenderId;
using LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeeName;
using LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeePatronymic;
using LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeeSurname;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Request.EmployeeService.Employee;

namespace LifeLine.Employee.Service.Api.Controllers.Api
{
    [ApiController]
    [Route("api/employees")]
    public class EmployeeController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request, CancellationToken cancellationToken = default)
        {
            var command = new CreateEmployeeCommand(request.Surname, request.Name, request.Patronymic, Guid.Parse(request.GenderId));

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное создание!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAllEmployeeQuery(), cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id}/update-employee")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateEmployeeRequest request, CancellationToken cancellationToken = default)
        {
            var command = new UpdateEmployeeCommand(id, request.Surname, request.Name, request.Patronymic, Guid.Parse(request.GenderId));

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное Обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch("{id}/update-surname")]
        public async Task<IActionResult> UpdateSurname([FromRoute] Guid id, [FromBody] string surname, CancellationToken cancellationToken = default)
        {
            var command = new UpdateEmployeeSurnameCommand(id, surname);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch("{id}/update-name")]
        public async Task<IActionResult> UpdateName([FromRoute] Guid id, [FromBody] string name, CancellationToken cancellationToken = default)
        {
            var command = new UpdateEmployeeNameCommand(id, name);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch("{id}/update-patronymic")]
        public async Task<IActionResult> UpdatePatronymic([FromRoute] Guid id, [FromBody] string patronymic, CancellationToken cancellationToken = default)
        {
            var command = new UpdateEmployeePatronymicCommand(id, patronymic);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpPatch("{id}/update-gender-id")]
        public async Task<IActionResult> UpdateGenderId([FromRoute] Guid id, [FromBody] Guid genderId, CancellationToken cancellationToken = default)
        {
            var command = new UpdateEmployeeGenderIdCommand(id, genderId);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное обновление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var command = new DeleteEmployeeCommand(id);

            var result = await _mediator.Send(command, cancellationToken);

            return result.Match<IActionResult>
                (
                    onSuccess: () => Ok("Успешное удаление!"),
                    onFailure: errors => BadRequest(errors)
                );
        }
    }
}
