using FluentValidation;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using Shared.Api.Application.Validators.Realization;

namespace LifeLine.Employee.Service.Application.Features.Employees.Create
{
    public sealed class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator() 
            => Include(new FIOValidator<CreateEmployeeCommand>(Surname.MAX_LENGTH, Name.MAX_LENGTH, Patronymic.MAX_LENGTH));
    }
}
