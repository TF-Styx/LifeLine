namespace Shared.Http.Interfaces
{
    public interface IGetAllHttpService<TResponse> where TResponse : class
    {
        Task<List<TResponse>> GetAllAsync();
    }
}
