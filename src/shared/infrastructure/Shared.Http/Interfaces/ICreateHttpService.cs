using Terminex.Common.Results;

namespace Shared.Http.Interfaces
{
    public interface ICreateHttpService
    {
        Task<Result> CreateAsync<TRequest>(TRequest request);
    }
}
