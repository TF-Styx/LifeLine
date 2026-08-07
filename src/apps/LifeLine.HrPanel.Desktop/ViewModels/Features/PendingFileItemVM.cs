using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.FilePreview;
using Shared.Contracts.Request.Files;
using Shared.WPF.Commands;
using Shared.WPF.Helpers;
using Shared.WPF.Services.FileDialog;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    public class PendingFileItemVM
    {
        private readonly IFileDialogService _fileDialogService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IFilePreviewService _filePreviewService;

        public PendingFileItemVM
            (
                IFileDialogService fileDialogService,
                IFileStorageService fileStorageService,
                IFilePreviewService filePreviewService
            )
        {
            _fileDialogService = fileDialogService;
            _fileStorageService = fileStorageService;
            _filePreviewService = filePreviewService;

            SelectMultipleCommand = new RelayCommand(Execute_SelectMultipleCommand);
            PreviewCommand = new RelayCommandAsync<PendingFileItem>(Execute_PreviewCommand);
            RemovePendingFileCommand = new RelayCommand<PendingFileItem>(Execute_RemovePendingFileCommand);
        }

        public ObservableCollection<PendingFileItem> PendingFilePaths { get; private set; } = [];

        public RelayCommand SelectMultipleCommand { get; private set; }
        private void Execute_SelectMultipleCommand()
        {
            var paths = _fileDialogService.GetFiles($"Выберите файлы", FileFilters.ImagesAndPdf);

            if (paths?.Any() == true)
            {
                var startIndex = PendingFilePaths.Count + 1;
                foreach (var path in paths)
                    PendingFilePaths.Add(new PendingFileItem(startIndex++, path));

                UpdateIndexes();
            }
        }

        public async Task LoadDocumentToQueueAsync(string fileKey)
        {
            PendingFilePaths.Clear();

            if (string.IsNullOrWhiteSpace(fileKey))
                return;

            var parsResult = S3UrlParser.Parse(fileKey);

            var metadataResult = await _fileStorageService.GetFileMetadataAsync(new GetFileMetadataRequest(parsResult.BuckerName!, parsResult.FileName!));

            if (metadataResult.IsFailure || metadataResult.Value == null)
            {
                MessageBox.Show($"Не удалось получить метаданные: {metadataResult.StringMessage}");
                return;
            }

            var pendingItem = PendingFileItem.FromMetadata(PendingFilePaths.Count + 1, metadataResult.Value, fileKey);

            PendingFilePaths.Add(pendingItem);
            UpdateIndexes();
        }

        public RelayCommandAsync<PendingFileItem>? PreviewCommand { get; private set; }
        private async Task Execute_PreviewCommand(PendingFileItem item)
        {
            if (item == null)
                return;

            try
            {
                string? tempPath = null;

                if (item.IsRemoteFile && !string.IsNullOrWhiteSpace(item.S3Url))
                    tempPath = await _filePreviewService.DownloadRemoteFileToTempAsync(item.S3Url, item.FileName);
                else if (!string.IsNullOrWhiteSpace(item.FilePath) && System.IO.File.Exists(item.FilePath))
                    tempPath = _filePreviewService.CopyLocalFileToTempAsync(item.FilePath, item.FileName);

                if (string.IsNullOrWhiteSpace(tempPath))
                {
                    MessageBox.Show("Не удалось подготовить файл для просмотра", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _filePreviewService.OpenInDefaultApplication(tempPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand<PendingFileItem>? RemovePendingFileCommand { get; private set; }
        private void Execute_RemovePendingFileCommand(PendingFileItem item)
        {
            if (item != null && PendingFilePaths.Remove(item))
                UpdateIndexes();
        }

        public void Clear() => PendingFilePaths.Clear();

        private void UpdateIndexes()
        {
            for (int i = 0; i < PendingFilePaths.Count; i++)
                PendingFilePaths[i].Index = i + 1;
        }
    }
}
