using FluentValidation;
using MediatR;
using Shared.Kernel.Results;
using System.Reflection;

namespace Shared.Api.Application.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse>
        (
            IEnumerable<IValidator<TRequest>> validators
        ) : IPipelineBehavior<TRequest, TResponse>
                where TRequest : IRequest<TResponse>
                where TResponse : Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next(cancellationToken);

            var context = new ValidationContext<TRequest>(request);

            var validationFailure = (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken))))
                .SelectMany(result => result.Errors).Where(failure => failure != null).ToList();

            if (!validationFailure.Any())
                return await next(cancellationToken);

            var errors = validationFailure.Select(x => new Error(ErrorCode.Validation, x.ErrorMessage)).ToList();

            var resultType = typeof(TResponse);

            MethodInfo? failureMethod = resultType.GetMethod(nameof(Result.Failure), BindingFlags.Public | BindingFlags.Static, [typeof(IEnumerable<Error>)]);

            return (TResponse)failureMethod?.Invoke(null, [errors])!;
        }
    }
}
