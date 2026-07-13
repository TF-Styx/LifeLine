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
    internal sealed class WorkPermitsVM : BaseEmployeeViewModel
    {
        private readonly IFileDialogService _fileDialogService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IFilePreviewService _filePreviewService;
        private readonly IDocumentProcessingService _documentProcessingService;

        private readonly IReadOnlyCollection<PermitTypeDisplay> _permitTypes;
        private readonly IReadOnlyCollection<AdmissionStatusDisplay> _admissionStatuses;

        public WorkPermitsVM
            (
                IFileDialogService fileDialogService,
                IFileStorageService fileStorageService,
                IFilePreviewService filePreviewService,
                IDocumentProcessingService documentProcessingService,

                IReadOnlyCollection<PermitTypeDisplay> permitTypes, 
                IReadOnlyCollection<AdmissionStatusDisplay> admissionStatuses
            )
        {
            _fileDialogService = fileDialogService;
            _fileStorageService = fileStorageService;
            _filePreviewService = filePreviewService;
            _documentProcessingService = documentProcessingService;

            _permitTypes = permitTypes;
            _admissionStatuses = admissionStatuses;

            NewWorkPermitDisplay();

            WorkPermitsView = CollectionViewSource.GetDefaultView(LocalWorkPermits);

            WorkPermitsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(WorkPermitDisplay.SaveStatus)));

            SelectMultipleCommand = new RelayCommand(Execute_SelectMultipleCommand);
            PreviewCommand = new RelayCommandAsync<PendingFileItem>(Execute_PreviewCommand);
            RemovePendingFileCommand = new RelayCommand<PendingFileItem>(Execute_RemovePendingFileCommand);
            AddWorkPermitCommandAsync = new RelayCommandAsync(Execute_AddWorkPermitCommandAsync, CanExecute_AddWorkPermitCommand);
        }

        private WorkPermitDisplay? _display;
        public WorkPermitDisplay? Display
        {
            get => _display;
            set
            {
                SetProperty(ref _display, value);

                AddWorkPermitCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void NewWorkPermitDisplay()
        {
            Display = new WorkPermitDisplay
                (
                    new WorkPermitResponse
                    (
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        string.Empty,
                        DateTime.Now,
                        DateTime.Now,
                        string.Empty,
                        string.Empty,
                        string.Empty
                    ),
                    _permitTypes,
                    _admissionStatuses,
                    SaveStatus.Local
                );

            Display.PropertyChanged += (s, e) => AddWorkPermitCommandAsync?.RaiseCanExecuteChanged();
        }

        private WorkPermitDisplay? _selectedLocalWorkPermit = null!;
        public WorkPermitDisplay? SelectedLocalWorkPermit
        {
            get => _selectedLocalWorkPermit;
            set
            {
                if (value != null)
                {
                    SetProp(value);

                    SetProperty(ref _selectedLocalWorkPermit, value);

                    _ = LoadDocumentToQueueAsync(value);
                }
            }
        }

        private void SetProp(WorkPermitDisplay value)
        {
            Display?.WorkPermitName = value.WorkPermitName;
            Display?.DocumentSeries = value.DocumentSeries;
            Display?.WorkPermitNumber = value.WorkPermitNumber;
            Display?.ProtocolNumber = value.ProtocolNumber;
            Display?.SpecialtyName = value.SpecialtyName;
            Display?.IssuingAuthority = value.IssuingAuthority;
            Display?.IssueDate = value.IssueDate;
            Display?.ExpiryDate = value.ExpiryDate;
            Display?.PermitType = value.PermitType;
            Display?.AdmissionStatus = value.AdmissionStatus;
        }

        private async Task LoadDocumentToQueueAsync(WorkPermitDisplay document)
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
                Debug.WriteLine($"[WorkPermitsVM] [Execute_PreviewCommand] item пуст!");
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

        public ObservableCollection<WorkPermitDisplay> LocalWorkPermits { get; private init; } = [];
        public ICollectionView WorkPermitsView { get; private init; } = null!;

        public RelayCommandAsync AddWorkPermitCommandAsync { get; private set; }
        private async Task Execute_AddWorkPermitCommandAsync()
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
                    Display.PermitType.Name,
                    EmployeeId!,
                    Display.WorkPermitNumber
                );

            if (processResult.IsFailure)
            {
                MessageBox.Show(processResult.StringMessage);
                return;
            }

            var (pdfBytes, fileName) = processResult.Value;

            LocalWorkPermits.Add
                (
                    new WorkPermitDisplay
                        (
                            new WorkPermitResponse
                                (
                                    string.Empty,
                                    EmployeeId!,
                                    Display.WorkPermitName,
                                    Display.DocumentSeries,
                                    Display.WorkPermitNumber,
                                    Display.ProtocolNumber,
                                    Display.SpecialtyName,
                                    Display.IssuingAuthority,
                                    Display.IssueDate,
                                    Display.ExpiryDate,
                                    null,
                                    Display.PermitType.Id,
                                    Display.AdmissionStatus.Id
                                ),
                            _permitTypes, _admissionStatuses, SaveStatus.Local
                        )
                    {
                        FileBytes = pdfBytes,
                        FileName = fileName,
                        ContentType = "application/pdf",
                    }
                );

            ClearProperty();
        }
        private bool CanExecute_AddWorkPermitCommand()
            => Display?.AdmissionStatus != null && Display?.PermitType != null &&
            !string.IsNullOrWhiteSpace(Display?.WorkPermitName) &&
            !string.IsNullOrWhiteSpace(Display?.WorkPermitNumber) &&
            !string.IsNullOrWhiteSpace(Display?.SpecialtyName) &&
            !string.IsNullOrWhiteSpace(Display?.IssuingAuthority) &&
            Display?.IssueDate != DateTime.MinValue && 
            !string.IsNullOrWhiteSpace(Display?.IssueDate.ToString()) &&
            Display?.ExpiryDate != DateTime.MinValue &&
            !string.IsNullOrWhiteSpace(Display?.ExpiryDate.ToString());

        public void ClearProperty()
        {
            Display?.WorkPermitName = string.Empty;
            Display?.DocumentSeries = string.Empty;
            Display?.WorkPermitNumber = string.Empty;
            Display?.ProtocolNumber = string.Empty;
            Display?.SpecialtyName = string.Empty;
            Display?.IssuingAuthority = string.Empty;
            Display?.IssueDate = DateTime.UtcNow;
            Display?.ExpiryDate = DateTime.UtcNow;
            Display?.PermitType = null!;
            Display?.AdmissionStatus = null!;

            PendingFilePaths.Clear();
            SelectedLocalWorkPermit = null;
        }

        private void UpdateIndexes()
        {
            for (int i = 0; i < PendingFilePaths.Count; i++)
                PendingFilePaths[i].Index = i + 1;
        }
    }
}
