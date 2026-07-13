using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.Document.DocumentProcessing;
using LifeLine.HrPanel.Desktop.Services.FilePreview;
using Shared.Contracts.Request.Files;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Enums;
using Shared.WPF.Helpers;
using Shared.WPF.Services.FileDialog;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class PersonalDocumentsVM : BaseEmployeeViewModel
    {
        private readonly IFileDialogService _fileDialogService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IFilePreviewService _filePreviewService;
        private readonly IDocumentProcessingService _documentProcessingService;

        private readonly IReadOnlyCollection<DocumentTypeDisplay> _documentTypes;


        public PersonalDocumentsVM
            (
                IFileDialogService fileDialogService,
                IFileStorageService fileStorageService,
                IFilePreviewService filePreviewService,
                IDocumentProcessingService documentProcessingService,

                IReadOnlyCollection<DocumentTypeDisplay> documentTypes
            )
        {
            _fileDialogService = fileDialogService;
            _fileStorageService = fileStorageService;
            _filePreviewService = filePreviewService;
            _documentProcessingService = documentProcessingService;

            _documentTypes = documentTypes;

            NewPersonalDocumentDisplay();

            PersonalDocumentsView = CollectionViewSource.GetDefaultView(LocalPersonalDocuments);

            PersonalDocumentsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(PersonalDocumentDisplay.SaveStatus)));

            SelectMultipleCommand = new RelayCommand(Execute_SelectMultipleCommand);
            PreviewCommand = new RelayCommandAsync<PendingFileItem>(Execute_PreviewCommand);
            RemovePendingFileCommand = new RelayCommand<PendingFileItem>(Execute_RemovePendingFileCommand);
            AddPersonalDocumentCommand = new RelayCommandAsync(Execute_AddPersonalDocumentCommand, CanExecute_AddPersonalDocumentCommand);
        }

        private PersonalDocumentDisplay? _display = null!;
        public PersonalDocumentDisplay? Display
        {
            get => _display;
            set
            {
                SetProperty(ref _display, value);

                AddPersonalDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        private void NewPersonalDocumentDisplay()
        {
            Display = new PersonalDocumentDisplay
                (
                    new PersonalDocumentResponse
                    (
                        Guid.Empty,
                        Guid.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty
                    ),
                    _documentTypes, SaveStatus.Local
                );

            Display.PropertyChanged += (s, e) => AddPersonalDocumentCommand?.RaiseCanExecuteChanged();
        }

        private PersonalDocumentDisplay? _selectedLocalPersonalDocument = null!;
        public PersonalDocumentDisplay? SelectedLocalPersonalDocument
        {
            get => _selectedLocalPersonalDocument;
            set
            {
                if (value != null)
                {
                    SetProp(value);

                    SetProperty(ref _selectedLocalPersonalDocument, value);

                    _ = LoadDocumentToQueueAsync(value);
                }
            }
        }

        private void SetProp(PersonalDocumentDisplay value)
        {
            Display?.DocumentNumber = value.DocumentNumber;
            Display?.DocumentSeries = value.DocumentSeries;
            Display?.DocumentType = value.DocumentType;
        }

        private async Task LoadDocumentToQueueAsync(PersonalDocumentDisplay document)
        {
            PendingFilePaths.Clear();

            if (document.SaveStatus != SaveStatus.DataBase)
                return;

            if (string.IsNullOrWhiteSpace(document.FileKey))
                return;

            var (bucketName, fileName) = S3UrlParser.Parse(document.FileKey);

            var metadataResult = await _fileStorageService.GetFileMetadataAsync(new GetFileMetadataRequest(bucketName!, fileName!));

            if (metadataResult.IsFailure || metadataResult.Value == null)
            {
                MessageBox.Show($"Не удалось получить метаданные: {metadataResult.StringMessage}");
                return;
            }

            var pendingItem = PendingFileItem.FromMetadata(PendingFilePaths.Count + 1, metadataResult.Value, document.FileKey);

            PendingFilePaths.Add(pendingItem);
            UpdateIndexes();
        }

        public ObservableCollection<PendingFileItem> PendingFilePaths { get; private set; } = [];

        public RelayCommand SelectMultipleCommand { get; private set; }
        private void Execute_SelectMultipleCommand()
        {
            var paths = _fileDialogService.GetFiles($"Выберите файлы: {FileDialogConsts.PERSONAL_DOCUMENT}", FileFilters.ImagesAndPdf);

            if (paths?.Any() == true)
            {
                var startIndex = PendingFilePaths.Count + 1;
                foreach (var path in paths)
                    PendingFilePaths.Add(new PendingFileItem(startIndex++, path));

                UpdateIndexes();
            }
        }

        public RelayCommandAsync<PendingFileItem>? PreviewCommand { get; private set; }
        private async Task Execute_PreviewCommand(PendingFileItem item)
        {
            if (item == null)
            {
                Debug.WriteLine($"[PersonalDocumentsVM] [Execute_PreviewCommand] item пуст!");
                return;
            }

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
                Debug.WriteLine($"[PreviewCommand] Ошибка: {ex.Message}");
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

        public ObservableCollection<PersonalDocumentDisplay> LocalPersonalDocuments { get; init; } = [];
        public ICollectionView PersonalDocumentsView { get; private init; } = null!;

        public RelayCommandAsync? AddPersonalDocumentCommand { get; private set; }
        private async Task Execute_AddPersonalDocumentCommand()
        {
            if (Display == null)
            {
                MessageBox.Show("Поля не заполнены!");
                return;
            }

            if (!PendingFilePaths.Any())
            {
                MessageBox.Show("Выберите хотя бы один файл для добавления", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var processResult = await _documentProcessingService.ProcessFilesToPdfAsync
                (
                    PendingFilePaths,
                    Display.DocumentType.Name,
                    EmployeeId!,
                    Display.DocumentNumber
                );

            if (processResult.IsFailure)
            {
                MessageBox.Show(processResult.StringMessage);
                return;
            }

            var (pdfBytes, fileName) = processResult.Value;

            LocalPersonalDocuments.Add
                (
                    new PersonalDocumentDisplay
                        (
                            new PersonalDocumentResponse
                                (
                                    Guid.Empty,
                                    Guid.Parse(Display.DocumentType.Id),
                                    Display.DocumentNumber,
                                    Display.DocumentSeries,
                                    null
                                ),
                            _documentTypes,
                            SaveStatus.Local
                        )
                    {
                        FileBytes = pdfBytes,
                        FileName = fileName,
                        ContentType = "application/pdf",
                    }
                );

            ClearProperty();
        }
        private bool CanExecute_AddPersonalDocumentCommand()
            => Display?.DocumentType != null && !string.IsNullOrWhiteSpace(Display.DocumentNumber);

        public void ClearProperty()
        {
            Display?.DocumentNumber = string.Empty;
            Display?.DocumentSeries = string.Empty;
            Display?.DocumentType = null!;

            PendingFilePaths.Clear();
            SelectedLocalPersonalDocument = null;
        }

        private void UpdateIndexes()
        {
            for (int i = 0; i < PendingFilePaths.Count; i++)
                PendingFilePaths[i].Index = i + 1;
        }
    }
}
