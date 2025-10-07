using FluentValidation;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using Shared.Api.Application.Validators.Realization;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeePatronymic
{
    public sealed class UpdateEmployeePatronymicCommandValidator : AbstractValidator<UpdateEmployeePatronymicCommand>
    {
        public UpdateEmployeePatronymicCommandValidator() => Include(new PatronymicValidator<UpdateEmployeePatronymicCommand>(Patronymic.MAX_LENGTH));
    }
}
