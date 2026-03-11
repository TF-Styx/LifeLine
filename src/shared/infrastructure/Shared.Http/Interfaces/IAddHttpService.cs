using Terminex.Common.Results;

namespace Shared.Http.Interfaces
{
    public interface IAddHttpService<TResponse> where TResponse : class
    {
        Task<Result<TResponse>> AddAsync<TRequest>(TRequest request);
        Task<Result<TCurrentResponse>> AddAsync<TRequest, TCurrentResponse>(TRequest request);
    }
}
