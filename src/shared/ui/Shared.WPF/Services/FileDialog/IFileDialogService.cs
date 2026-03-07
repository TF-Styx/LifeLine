namespace Shared.WPF.Services.FileDialog
{
    public interface IFileDialogService
    {
        string GetFile(string title, string filter);
        IEnumerable<string> GetFiles(string title, string filter);
    }
}