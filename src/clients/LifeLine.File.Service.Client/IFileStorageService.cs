using Shared.Contracts.Request.Files;
using Shared.Contracts.Response.Files;
using Terminex.Common.Results;

namespace LifeLine.File.Service.Client
{
    public interface IFileStorageService
    {
        Task<Result<UploadFileResponse?>> UploadFileAsync(UploadFileRequest request);
        Task<string> GetLink(string key);
    }
}
