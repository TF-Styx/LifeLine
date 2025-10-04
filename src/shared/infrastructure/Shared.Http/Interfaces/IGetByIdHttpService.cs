namespace Shared.Http.Interfaces
{
    public interface IGetByIdHttpService<TResponse, in TKey> where TResponse : class
    {
        Task<TResponse?> GetByIdAsync(TKey id);
    }
}
