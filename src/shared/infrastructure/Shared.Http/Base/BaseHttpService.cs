using Shared.Kernel.Results;
using System.Net.Http.Json;
using System.Text.Json;

namespace Shared.Http.Base
{
    public abstract class BaseHttpService<TResponse, TKey>(HttpClient httpClient, string url) 
        : BaseReadHttpService<TResponse, TKey>(httpClient, url), IBaseHttpService<TResponse, TKey>
            where TResponse : class
    {
        public virtual async Task<Result<TResponse>> AddAsync<TRequest>(TRequest request)
        {
            HttpResponseMessage response = null!;

            try
            {
                response = await HttpClient.PostAsJsonAsync(Url, request, JsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<TResponse>(JsonSerializerOptions);

                if (result == null)
                    return Result<TResponse>.Failure(new Error(ErrorCode.InvalidResponse, $"Не удалось преобразовать ответ! - {typeof(TResponse)}"));

                return Result<TResponse>.Success(result);
            }
            catch(HttpRequestException ex)
            {
                if (response == null)
                    return Result<TResponse>.Failure(new Error(ErrorCode.Create, $"Сетевая ошибка добавления элемента в {Url} : {ex.Message}"));

                return Result<TResponse>.Failure(new Error(ErrorCode.Create, $"Ошибка добавления элемента в {Url} : {response.StatusCode} - {await response.Content.ReadAsStringAsync()}"));
            }
            catch (JsonException jsonEx)
            {
                return Result<TResponse>.Failure(new Error(ErrorCode.Create, $"Ошибка десериализации ответа от {Url}: {jsonEx.Message}"));
            }
            catch (Exception ex)
            {
                return Result<TResponse>.Failure(new Error(ErrorCode.Create, $"Непредвиденная ошибка добавления элемента в {Url} : {ex.Message}"));
            }
        }

        public virtual async Task<Result> UpdateAsync<TRequest>(TKey id, TRequest request)
        {
            HttpResponseMessage response = null!;

            try
            {
                response = await HttpClient.PatchAsJsonAsync($"{Url}/{id}", request, JsonSerializerOptions);
                response.EnsureSuccessStatusCode();

                if (!response.IsSuccessStatusCode)
                    return Result.Failure(new Error(ErrorCode.Update, await response.Content.ReadAsStringAsync()));

                return Result.Success();
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                    return Result.Failure(new Error(ErrorCode.Update, $"Сетевая ошибка обновления элемента в {Url} : {ex.Message}"));

                return Result.Failure(new Error(ErrorCode.Update, $"Ошибка обновления элемента в {Url} : {response.StatusCode} - {await response.Content.ReadAsStringAsync()}"));
            }
            catch (JsonException jsonEx)
            {
                return Result.Failure(new Error(ErrorCode.Update, $"Ошибка десериализации ответа от {Url}: {jsonEx.Message}"));
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Update, $"Непредвиденная ошибка обновления элемента в {Url} : {ex.Message}"));
            }
        }

        public virtual async Task<Result> DeleteAsync(TKey id)
        {
            HttpResponseMessage response = null!;

            try
            {
                response = await HttpClient.DeleteAsync($"{Url}/{id}");
                response.EnsureSuccessStatusCode();

                return Result.Success();
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                    return Result.Failure(new Error(ErrorCode.Delete, $"Сетевая ошибка удаления элемента в {Url} : {ex.Message}"));

                return Result.Failure(new Error(ErrorCode.Delete, $"Ошибка удаления элемента в {Url} : {response.StatusCode} - {await response.Content.ReadAsStringAsync()}"));
            }
            catch (JsonException jsonEx)
            {
                return Result.Failure(new Error(ErrorCode.Delete, $"Ошибка десериализации ответа от {Url}: {jsonEx.Message}"));
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Delete, $"Непредвиденная ошибка удаления элемента в {Url} : {ex.Message}"));
            }
        }
    }
}
