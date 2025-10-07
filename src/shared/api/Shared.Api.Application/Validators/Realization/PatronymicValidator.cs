using FluentValidation;
using Shared.Api.Application.Validators.Abstraction;

namespace Shared.Api.Application.Validators.Realization
{
    public sealed class PatronymicValidator<TCommand> : AbstractValidator<TCommand> where TCommand : IHasPatronymic
    {
        public PatronymicValidator(int maxLength)
        {
            RuleFor(x => x.Patronymic)
                .NotEmpty()
                .WithMessage("Поле отчества не должно быть пустым!")
                .MaximumLength(maxLength)
                .WithMessage($"Поля отчества не должно превышать данного значения - {maxLength}!");
        }
    }
}
