using FluentValidation;
using Shared.Api.Application.Validators.Abstraction;

namespace Shared.Api.Application.Validators.Realization
{
    public sealed class NameValidator<TName> : AbstractValidator<TName> where TName : IHasName
    {
        public NameValidator(int maxLength)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Поле имя не должно быть пустым!")
                .MaximumLength(maxLength)
                .WithMessage($"Поля имя не должно превышать данного значения - {maxLength}!");
        }
    }
}
