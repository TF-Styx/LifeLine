using Shared.Kernel.Results;

namespace Shared.Http.Interfaces
{
    public interface IDeleteHttpService<in TKey>
    {
        Task<Result> DeleteAsync(TKey id);
    }
}
