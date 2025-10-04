using Shared.Kernel.Results;

namespace Shared.Http.Interfaces
{
    public interface IAddHttpService<TResponse> where TResponse : class
    {
        Task<Result<TResponse>> AddAsync<TRequest>(TRequest request);
    }
}
