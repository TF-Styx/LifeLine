namespace Shared.Http.Base
{
    public interface IBaseHttpService<TResponse, TKey> : IBaseReadHttpService<TResponse, TKey>, IBaseWriteHttpService<TResponse, TKey> where TResponse : class;
}
