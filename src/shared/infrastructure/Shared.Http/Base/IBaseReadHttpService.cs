using Shared.Http.Interfaces;

namespace Shared.Http.Base
{
    public interface IBaseReadHttpService<TResponse, in TKey>
        : IGetAllHttpService<TResponse>, IGetByIdHttpService<TResponse, TKey>
        where TResponse : class;
}
