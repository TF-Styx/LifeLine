using System.Net.Http.Json;
using System.Text.Json;
using Terminex.Common.Results;

namespace Shared.Http.Base
{
    public abstract class BaseHttpService<TResponse, TKey>(HttpClient httpClient, string url, JsonSerializerOptions options) 
        : BaseReadHttpService<TResponse, TKey>(httpClient, url, options), IBaseHttpService<TResponse, TKey>
            where TResponse : class
    {
        public virtual async Task<Result> CreateAsync<TRequest>(TRequest request)
        {
            HttpResponseMessage response = null!;

            try
            {
                response = await HttpClient.PostAsJsonAsync(Url, request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                {
                    var errors = await DeserializeErrorsAsync(response);
                    return Result.Failure(errors);
                }

                return Result.Success();
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                    return Result.Failure(new Error(ErrorCode.Create, $"Сетевая ошибка добавления элемента в {Url} : {ex.Message}"));

                return Result.Failure(new Error(ErrorCode.Create, $"Ошибка добавления элемента в {Url} : {response.StatusCode} - {await response.Content.ReadAsStringAsync()}"));
            }
            catch (JsonException jsonEx)
            {
                return Result.Failure(new Error(ErrorCode.Create, $"Ошибка десериализации ответа от {Url}: {jsonEx.Message}"));
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Create, $"Непредвиденная ошибка добавления элемента в {Url} : {ex.Message}"));
            }
        }

        public virtual async Task<Result<TResponse>> AddAsync<TRequest>(TRequest request)
        {
            HttpResponseMessage response = null!;

            try
            {
                response = await HttpClient.PostAsJsonAsync(Url, request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                {
                    var errors = await DeserializeErrorsAsync(response);
                    return Result<TResponse>.Failure(errors);
                }

                return Result<TResponse>.Success(await response.Content.ReadFromJsonAsync<TResponse>(JsonSerializerOptions));
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

        public virtual async Task<Result<TCurrentResponse>> AddAsync<TRequest, TCurrentResponse>(TRequest request)
        {
            HttpResponseMessage response = null!;

            try
            {
                response = await HttpClient.PostAsJsonAsync(Url, request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                {
                    var errors = await DeserializeErrorsAsync(response);
                    return Result<TCurrentResponse>.Failure(errors);
                }

                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(content))
                    return default!;

                if (typeof(TCurrentResponse) == typeof(string))
                {
                    if (content.StartsWith("\"") && content.EndsWith("\""))
                    {
                        using var doc = JsonDocument.Parse(content);
                        var value = doc.RootElement.GetString();
                        return Result<TCurrentResponse>.Success((TCurrentResponse)(object)value!);
                    }
                    else
                    {
                        return Result<TCurrentResponse>.Success((TCurrentResponse)(object)content);
                    }
                }

                return Result<TCurrentResponse>.Success(await response.Content.ReadFromJsonAsync<TCurrentResponse>(JsonSerializerOptions));
            }
            catch (HttpRequestException ex)
            {
                if (response == null)
                    return Result<TCurrentResponse>.Failure(new Error(ErrorCode.Create, $"Сетевая ошибка добавления элемента в {Url} : {ex.Message}"));

                return Result<TCurrentResponse>.Failure(new Error(ErrorCode.Create, $"Ошибка добавления элемента в {Url} : {response.StatusCode} - {await response.Content.ReadAsStringAsync()}"));
            }
            catch (JsonException jsonEx)
            {
                return Result<TCurrentResponse>.Failure(new Error(ErrorCode.Create, $"Ошибка десериализации ответа от {Url}: {jsonEx.Message}"));
            }
            catch (Exception ex)
            {
                return Result<TCurrentResponse>.Failure(new Error(ErrorCode.Create, $"Непредвиденная ошибка добавления элемента в {Url} : {ex.Message}"));
            }
        }

        public virtual async Task<Result> UpdateAsync<TRequest>(TKey id, TRequest request)
        {
            HttpResponseMessage response = null!;

            try
            {
                response = await HttpClient.PatchAsJsonAsync($"{Url}/{id}", request, JsonSerializerOptions);

                if (!response.IsSuccessStatusCode)
                {
                    var errors = await DeserializeErrorsAsync(response);
                    return Result<TResponse>.Failure(errors);
                }

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

                if (!response.IsSuccessStatusCode)
                {
                    var errors = await DeserializeErrorsAsync(response);
                    return Result<TResponse>.Failure(errors);
                }

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

        /// <summary>
        /// Десериализует список ошибок из HTTP ответа
        /// </summary>
        private async Task<IReadOnlyList<Error>> DeserializeErrorsAsync(HttpResponseMessage response)
        {
            try
            {
                var errors = await response.Content.ReadFromJsonAsync<List<Error>>(JsonSerializerOptions);
                return errors?.AsReadOnly() ?? new List<Error>().AsReadOnly();
            }
            catch
            {
                // Если не удалось десериализовать, создаём общую ошибку
                var content = await response.Content.ReadAsStringAsync();
                return new List<Error> 
                { 
                    new Error
                    (
                        ErrorCode.InvalidResponse, 
                        $"Ошибка {response.StatusCode}: {content}"
                    ) 
                }.AsReadOnly();
            }
        }
    }
}
