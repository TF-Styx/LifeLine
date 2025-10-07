using FluentValidation;
using Shared.Api.Application.Validators.Abstraction;

namespace Shared.Api.Application.Validators.Realization
{
    public sealed class FIOValidator<TCommand> : AbstractValidator<TCommand> where TCommand : IHasFIO
    {
        public FIOValidator(int maxSurnameLength, int maxNameLength, int maxPatronymicLength)
        {
            Include(new SurnameValidator<TCommand>(maxSurnameLength));
            Include(new NameValidator<TCommand>(maxNameLength));
            Include(new PatronymicValidator<TCommand>(maxPatronymicLength));
        }
    }
}
