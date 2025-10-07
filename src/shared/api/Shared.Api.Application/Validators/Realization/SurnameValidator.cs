using FluentValidation;
using Shared.Api.Application.Validators.Abstraction;

namespace Shared.Api.Application.Validators.Realization
{
    public sealed class SurnameValidator<TCommand> : AbstractValidator<TCommand> where TCommand : IHasSurname
    {
        public SurnameValidator(int maxLength)
        {
            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithMessage("Поле фамилии не должно быть пустым!")
                .MaximumLength(maxLength)
                .WithMessage($"Поля фамилии не должно превышать данного значения - {maxLength}!");
        }
    }
}
