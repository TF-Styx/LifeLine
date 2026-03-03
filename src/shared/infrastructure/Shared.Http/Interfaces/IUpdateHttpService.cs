using Terminex.Common.Results;

namespace Shared.Http.Interfaces
{
    public interface IUpdateHttpService<in TKey>
    {
        Task<Result> UpdateAsync<TRequest>(TKey id, TRequest request);
    }
}
