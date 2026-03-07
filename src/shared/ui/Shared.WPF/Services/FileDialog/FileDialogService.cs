using Microsoft.Win32;
using System.Windows;

namespace Shared.WPF.Services.FileDialog
{
    public sealed class FileDialogService : IFileDialogService
    {
        public string GetFile(string title, string filter)
        {
            try
            {
                var dialog = new OpenFileDialog()
                {
                    Title = title,
                    Filter = filter,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Multiselect = false
                };

                if (dialog.ShowDialog() == true)
                    return dialog.FileName;

                return string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при открытии файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }
        }

        public IEnumerable<string> GetFiles(string title, string filter)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при открытии файлов: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return [];
            }
        }
    }
}
