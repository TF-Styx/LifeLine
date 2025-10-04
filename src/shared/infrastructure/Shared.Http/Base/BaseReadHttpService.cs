using System.Net.Http.Json;
using System.Text.Json;

namespace Shared.Http.Base
{
    public abstract class BaseReadHttpService<TResponse, TKey> : IBaseReadHttpService<TResponse, TKey> where TResponse : class
    {
        public readonly HttpClient HttpClient;
        public readonly string Url;
        protected readonly JsonSerializerOptions JsonSerializerOptions;

        protected BaseReadHttpService(HttpClient httpClient, string url)
        {
            HttpClient = httpClient;
            Url = url;
            JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
        }

        public virtual async Task<List<TResponse>> GetAllAsync()
        {
            try
            {
                var response = await HttpClient.GetAsync(Url);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<List<TResponse>>(JsonSerializerOptions);

                return result ?? [];
            }
            catch (Exception)
            {
                // TODO : Добавить логирование
                return [];
            }
        }

        public virtual async Task<TResponse?> GetByIdAsync(TKey id)
        {
            try
            {
                var response = await HttpClient.GetAsync($"{Url}/{id}");
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<TResponse>(JsonSerializerOptions);

                return result ?? null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
