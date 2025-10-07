using FluentValidation;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using Shared.Api.Application.Validators.Realization;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployee
{
    public sealed class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator() => Include(new FIOValidator<UpdateEmployeeCommand>(Surname.MAX_LENGTH, Name.MAX_LENGTH, Patronymic.MAX_LENGTH));
    }
}
