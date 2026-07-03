using MediatR;
using Terminex.Common.Results;
using Shared.Kernel.Exceptions;
using Microsoft.Extensions.Logging;

namespace Shared.Application.Behaviours
{
    public sealed class ExceptionBehaviour<TRequest, TResponse>(ILogger<ExceptionBehaviour<TRequest, TResponse>> logger) 
        : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse> 
        where TResponse : Result, IResultWithFactory<TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next(cancellationToken);
            }
            catch (DomainException ex)
            {
                logger.LogWarning(ex, "Доменная ошибка при обработке {RequestType}", typeof(TRequest).Name);

                var error = Error.Validation(ex.Message);

                return TResponse.CreateFailure([error]);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Необработанное исключение при обработке запроса {RequestType}", typeof(TRequest).Name);

                var error = Error.Server("Ошибка на стороне сервера");

                return TResponse.CreateFailure([error]);
            }
        }
    }
}
