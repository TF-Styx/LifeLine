namespace LifeLine.File.Service.Client
{
    public interface IFileStorageService
    {
        Task<string> GetLink(string key);
    }
}
