namespace Shared.Contracts.Request.Files
{
    public sealed record UploadFilesRequest(List<UploadFilesDataRequest> Files);

    public sealed record UploadFilesDataRequest
        (
            string BucketName,
            string AdditionalName,
            string? SubFolder,
            string? FilePath = null,           // Для загрузки с диска
            byte[]? FileBytes = null,          // Для загрузки из памяти
            string? FileName = null,           // Обязателен при использовании FileBytes
            string? ContentType = null         // MIME-тип, по умолчанию определяется автоматически
        );
}
