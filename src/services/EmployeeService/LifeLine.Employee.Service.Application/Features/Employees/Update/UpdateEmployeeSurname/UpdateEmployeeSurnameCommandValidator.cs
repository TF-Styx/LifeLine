using FluentValidation;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using Shared.Api.Application.Validators.Realization;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeeSurname
{
    public sealed class UpdateEmployeeSurnameCommandValidator : AbstractValidator<UpdateEmployeeSurnameCommand>
    {
        public UpdateEmployeeSurnameCommandValidator() => Include(new SurnameValidator<UpdateEmployeeSurnameCommand>(Surname.MAX_LENGTH));
    }
}
