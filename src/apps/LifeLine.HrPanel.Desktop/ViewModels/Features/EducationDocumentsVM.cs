using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Helpers;
using Shared.WPF.Services.Conversion;
using Shared.WPF.Services.FileDialog;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class EducationDocumentsVM : BaseEmployeeViewModel
    {
        private readonly IFileDialogService _fileDialogService;
        private readonly IDocumentConversionService _documentConversionService;

        private readonly IReadOnlyCollection<DocumentTypeDisplay> _documentTypes;
        private readonly IReadOnlyCollection<EducationLevelDisplay> _educationLevels;

        public EducationDocumentsVM
            (
                IFileDialogService fileDialogService,
                IDocumentConversionService documentConversionService,
                IReadOnlyCollection<DocumentTypeDisplay> documentTypes,
                IReadOnlyCollection<EducationLevelDisplay> educationLevels
            )
        {
            _fileDialogService = fileDialogService;
            _documentConversionService = documentConversionService;

            _documentTypes = documentTypes;
            _educationLevels = educationLevels;

            SelectMultipleCommand = new RelayCommand(Execute_SelectMultipleCommand);
            AddEducationDocumentCommandAsync = new RelayCommandAsync(Execute_AddEducationDocumentCommandAsync, CanExecute_AddEducationDocumentCommand);
            DeleteEducationDocumentCommand = new RelayCommand<EducationDocumentDisplay>(Execute_DeleteEducationDocumentCommand);
        }

        private string _documentNumber = null!;
        public string DocumentNumber
        {
            get => _documentNumber;
            set
            {
                SetProperty(ref _documentNumber, value);
                AddEducationDocumentCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private DateTime _issuedDate;
        public DateTime IssuedDate
        {
            get => _issuedDate;
            set
            {
                SetProperty(ref _issuedDate, value);
                AddEducationDocumentCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private string _organizationName = null!;
        public string OrganizationName
        {
            get => _organizationName;
            set
            {
                SetProperty(ref _organizationName, value);
                AddEducationDocumentCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private string? _qualificationAwardedName;
        public string? QualificationAwardedName
        {
            get => _qualificationAwardedName;
            set
            {
                SetProperty(ref _qualificationAwardedName, value);
                AddEducationDocumentCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private string? _specialtyName;
        public string? SpecialtyName
        {
            get => _specialtyName;
            set
            {
                SetProperty(ref _specialtyName, value);
                AddEducationDocumentCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private string? _programName;
        public string? ProgramName
        {
            get => _programName;
            set
            {
                SetProperty(ref _programName, value);
                AddEducationDocumentCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private TimeSpan? _totalHours;
        public TimeSpan? TotalHours
        {
            get => _totalHours;
            set
            {
                SetProperty(ref _totalHours, value);
                AddEducationDocumentCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private EducationLevelDisplay _educationLevel = null!;
        public EducationLevelDisplay EducationLevel
        {
            get => _educationLevel;
            set
            {
                SetProperty(ref _educationLevel, value);
                AddEducationDocumentCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private DocumentTypeDisplay _documentType = null!;
        public DocumentTypeDisplay DocumentType
        {
            get => _documentType;
            set
            {
                SetProperty(ref _documentType, value);
                AddEducationDocumentCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        public string? FilePath
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        private EducationDocumentDisplay _selectedEducationDocumentDisplay = null!;
        public EducationDocumentDisplay SelectedEducationDocumentDisplay
        {
            get => _selectedEducationDocumentDisplay;
            set => SetProperty(ref _selectedEducationDocumentDisplay, value);
        }

        public ObservableCollection<string> PendingFilePaths { get; private set; } = [];

        public RelayCommand SelectMultipleCommand { get; private set; }
        private void Execute_SelectMultipleCommand()
        {
            var paths = _fileDialogService.GetFiles($"Выберите файлы: {FileDialogConsts.ASSIGNMENT}", FileFilters.ImagesAndPdf);

            if (paths.Any())
                foreach (var path in paths)
                    PendingFilePaths.Add(path);
        }

        public ObservableCollection<EducationDocumentDisplay> LocalEducationDocuments { get; private init; } = [];

        public RelayCommandAsync AddEducationDocumentCommandAsync { get; private set; }
        private async Task Execute_AddEducationDocumentCommandAsync()
        {
            var filesToProcess = PendingFilePaths.Any() ? [.. PendingFilePaths] : (FilePath != null ? [FilePath] : Array.Empty<string>());

            if (!filesToProcess.Any())
            {
                MessageBox.Show("Выберите хотя бы один файл для добавления", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                byte[] pdfBytes;

                if (filesToProcess.Count() > 1)
                {
                    var images = new List<byte[]>();

                    foreach (var path in filesToProcess)
                        if (System.IO.File.Exists(path))
                            images.Add(await System.IO.File.ReadAllBytesAsync(path));

                    if (!images.Any())
                    {
                        MessageBox.Show("Не удалось прочитать выбранные файлы", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    pdfBytes = await _documentConversionService.ConvertImagesToPdfAsync(images, DocumentType.Name, EmployeeId!);
                }
                else
                {
                    var imageBytes = await System.IO.File.ReadAllBytesAsync(filesToProcess.First());
                    pdfBytes = await _documentConversionService.ConvertImagesToPdfAsync([imageBytes], DocumentType.Name, EmployeeId!);
                }

                var fileName = ".pdf";

                LocalEducationDocuments.Add
                    (
                        new EducationDocumentDisplay
                            (
                                new EducationDocumentResponse
                                    (
                                        string.Empty,
                                        EmployeeId,
                                        EducationLevel.Id,
                                        DocumentType.Id,
                                        DocumentNumber,
                                        IssuedDate.ToString(),
                                        OrganizationName,
                                        QualificationAwardedName,
                                        SpecialtyName,
                                        ProgramName,
                                        TotalHours.ToString()
                                    ),
                                _educationLevels, 
                                _documentTypes, 
                                FilePath
                            )
                        {
                            FileBytes = pdfBytes,
                            FileName = fileName,
                            ContentType = "application/pdf"
                        }
                    );

                ClearProperty();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обработке файла: {ex.Message}", "Ошибка конвертации",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanExecute_AddEducationDocumentCommand()
            => EducationLevel != null && DocumentType != null &&
               !string.IsNullOrWhiteSpace(DocumentNumber) &&
               !string.IsNullOrWhiteSpace(IssuedDate.ToString()) &&
               IssuedDate != DateTime.MinValue &&
               !string.IsNullOrWhiteSpace(OrganizationName);

        public RelayCommand<EducationDocumentDisplay>? DeleteEducationDocumentCommand { get; private set; }
        private void Execute_DeleteEducationDocumentCommand(EducationDocumentDisplay display)
            => LocalEducationDocuments.Remove(display);

        public void ClearProperty()
        {
            DocumentNumber = string.Empty;
            IssuedDate = DateTime.UtcNow;
            OrganizationName = string.Empty;
            QualificationAwardedName = string.Empty;
            SpecialtyName = string.Empty;
            ProgramName = string.Empty;
            TotalHours = TimeSpan.Zero;
            EducationLevel = null!;
            DocumentType = null!;
            FilePath = string.Empty;

            PendingFilePaths.Clear();
        }
    }
}
