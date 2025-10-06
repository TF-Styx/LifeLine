using FluentValidation;
using LifeLine.Employee.Service.Domain.ValueObjects.Genders;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using Shared.Api.Application.Validators.Realization;

namespace LifeLine.Employee.Service.Application.Features.Genders.Update
{
    public sealed class UpdateGenderNameCommandValidator : AbstractValidator<UpdateGenderNameCommand>
    {
        public UpdateGenderNameCommandValidator(IGenderRepository repository)
        {
            Include(new NameValidator<UpdateGenderNameCommand>(GenderName.MAX_LENGTH));

            When(g => !string.IsNullOrWhiteSpace(g.Name), () => RuleFor(g => g.Name)
                .MustAsync(async (name, clt) => !await repository.ExistNameAsync(name))
                .WithMessage("Наименование гендера уже есть с таким именем!"));
        }
    }
}
