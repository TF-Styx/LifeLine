using Shared.Contracts.Request.Files;
using Shared.Contracts.Response.Files;
using Shared.Kernel.Errors;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Terminex.Common.Results;

namespace LifeLine.File.Service.Client
{
    internal class FileStorageService(HttpClient httpClient) : IFileStorageService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<Result<UploadFileResponse?>> UploadFileAsync(UploadFileRequest request)
        {
            using var formData = new MultipartFormDataContent
                {
                    { new StringContent(request.BucketName), nameof(request.BucketName) },
                    { new StringContent(request.AdditionalName), nameof(request.AdditionalName) }
                };

            if (!string.IsNullOrWhiteSpace(request.SubFolder))
                formData.Add(new StringContent(request.SubFolder, Encoding.UTF8, "text/plain"), nameof(request.SubFolder));

            if (!string.IsNullOrWhiteSpace(request.FilePath) && System.IO.File.Exists(request.FilePath))
            {
                var fileStream = System.IO.File.OpenRead(request.FilePath);
                var streamContent = new StreamContent(fileStream);

                var mineType = GetMimeType(request.FilePath);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(mineType);

                formData.Add(streamContent, "File", Path.GetFileName(request.FilePath));
            }

            try
            {
                var response = await _httpClient.PostAsync($"files", formData);

                response.EnsureSuccessStatusCode();

                return Result<UploadFileResponse?>.Success(await response.Content.ReadFromJsonAsync<UploadFileResponse?>());
            }
            catch (Exception ex)
            {
                return Result<UploadFileResponse?>.Failure(Error.New(AppErrors.Upload, $"Ошибка загрузки изображения!\n{ex.Message}"));
            }
        }

        public async Task<Result<List<UploadFileResponse>?>> UploadFilesAsync(UploadFilesRequest request)
        {
            using var formData = new MultipartFormDataContent();

            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];
                var prefix = $"Files[{i}]";

                formData.Add(new StringContent(file.BucketName), $"{prefix}.BucketName");
                formData.Add(new StringContent(file.AdditionalName), $"{prefix}.AdditionalName");

                if (!string.IsNullOrWhiteSpace(file.SubFolder))
                    formData.Add(new StringContent(file.SubFolder, Encoding.UTF8, "text/plain"), $"{prefix}.SubFolder");

                if (!string.IsNullOrWhiteSpace(file.FilePath) && System.IO.File.Exists(file.FilePath))
                {
                    var fileStream = System.IO.File.OpenRead(file.FilePath);
                    var streamContent = new StreamContent(fileStream);
                    var mineType = GetMimeType(file.FilePath);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(mineType);
                    formData.Add(streamContent, $"{prefix}.File", Path.GetFileName(file.FilePath));
                }
            }

            try
            {
                var response = await _httpClient.PostAsync("files/batch", formData);
                response.EnsureSuccessStatusCode();
                return Result<List<UploadFileResponse>?>.Success(await response.Content.ReadFromJsonAsync<List<UploadFileResponse>>());
            }
            catch (Exception ex)
            {
                return Result<List<UploadFileResponse>?>.Failure(Error.New(AppErrors.Upload, $"Ошибка загрузки!\n{ex.Message}"));
            }
        }

        public async Task<string> GetLink(string key)
        {
            var encodeKey = Uri.EscapeDataString(key);

            var response = await _httpClient.GetAsync($"api/files/link?key={encodeKey}");

            return await response.Content.ReadAsStringAsync();
        }

        private readonly Dictionary<string, string> MimeMappings = new(StringComparer.OrdinalIgnoreCase)
        {
            { ".png", "image/png" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".gif", "image/gif" },
            { ".bmp", "image/bmp" },
            { ".svg", "image/svg+xml" },
            { ".pdf", "application/pdf" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" }
        };

        public string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath);

            if (extension == null || !MimeMappings.TryGetValue(extension, out var mimeType))
                return "application/octet-stream";

            return mimeType;
        }
    }
}
