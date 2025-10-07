using FluentValidation;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using Shared.Api.Application.Validators.Realization;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployeeName
{
    public sealed class UpdateEmployeeNameCommandValidator : AbstractValidator<UpdateEmployeeNameCommand>
    {
        public UpdateEmployeeNameCommandValidator() => Include(new NameValidator<UpdateEmployeeNameCommand>(Name.MAX_LENGTH));
    }
}
