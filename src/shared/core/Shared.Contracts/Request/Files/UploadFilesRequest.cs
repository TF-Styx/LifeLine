namespace Shared.Contracts.Request.Files
{
    public sealed record UploadFilesRequest(List<UploadFilesDataRequest> Files);

    public sealed record UploadFilesDataRequest(string BucketName, string AdditionalName, string? SubFolder, string FilePath);
}
