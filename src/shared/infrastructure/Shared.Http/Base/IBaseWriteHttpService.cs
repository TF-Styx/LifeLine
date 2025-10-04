using Shared.Http.Interfaces;

namespace Shared.Http.Base
{
    public interface IBaseWriteHttpService<TResponse, in TKey>
        : IAddHttpService<TResponse>, IUpdateHttpService<TKey>, IDeleteHttpService<TKey>
        where TResponse : class;
}
