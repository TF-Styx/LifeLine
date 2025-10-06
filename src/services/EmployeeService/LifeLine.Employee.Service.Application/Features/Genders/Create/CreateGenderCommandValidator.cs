using FluentValidation;
using LifeLine.Employee.Service.Domain.ValueObjects.Genders;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using Shared.Api.Application.Validators.Realization;

namespace LifeLine.Employee.Service.Application.Features.Genders.Create
{
    public sealed class CreateGenderCommandValidator : AbstractValidator<CreateGenderCommand>
    {
        public CreateGenderCommandValidator(IGenderRepository repository)
        {
            Include(new NameValidator<CreateGenderCommand>(GenderName.MAX_LENGTH));

            When(g => !string.IsNullOrWhiteSpace(g.Name), () => RuleFor(g => g.Name)
                .MustAsync(async (name, clt) => !await repository.ExistNameAsync(name))
                .WithMessage("Наименование гендера уже есть с таким именем!"));
        }
    }
}
