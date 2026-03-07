using Microsoft.Win32;

namespace Shared.WPF.Services.FileDialog
{
    public sealed class FileDialogService : IFileDialogService
    {
        public string? GetFile(string title, string filter)
        {
            var dialog = new OpenFileDialog()
            {
                Title = title,
                Filter = filter,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            return (dialog.ShowDialog() == true) ? dialog.FileName : null;
        }

        public IEnumerable<string> GetFiles(string title, string filter)
        {
            var dialog = new OpenFileDialog()
            {
                Title = title,
                Filter = filter,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = true
            };

            if (dialog.ShowDialog() == true)
                return dialog.FileNames;

            return [];
        }
    }
}
