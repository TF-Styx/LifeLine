namespace Shared.Contracts.Request.Files
{
    public sealed record UploadFileRequest(string BucketName, string AdditionalName, string? SubFolder, string FilePath);
}
