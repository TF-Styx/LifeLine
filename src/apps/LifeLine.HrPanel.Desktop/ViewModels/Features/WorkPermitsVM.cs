using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Helpers;
using Shared.WPF.Services.Conversion;
using Shared.WPF.Services.FileDialog;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class WorkPermitsVM : BaseEmployeeViewModel
    {
        private readonly IFileDialogService _fileDialogService;
        private readonly IDocumentConversionService _documentConversionService;

        private readonly IReadOnlyCollection<PermitTypeDisplay> _permitTypes;
        private readonly IReadOnlyCollection<AdmissionStatusDisplay> _admissionStatuses;

        public WorkPermitsVM
            (
                IFileDialogService fileDialogService, 
                IDocumentConversionService documentConversionService,
                IReadOnlyCollection<PermitTypeDisplay> permitTypes, 
                IReadOnlyCollection<AdmissionStatusDisplay> admissionStatuses
            )
        {
            _fileDialogService = fileDialogService;
            _documentConversionService = documentConversionService;

            _permitTypes = permitTypes;
            _admissionStatuses = admissionStatuses;

            SelectMultipleCommand = new RelayCommand(Execute_SelectMultipleCommand);
            AddWorkPermitCommandAsync = new RelayCommandAsync(Execute_AddWorkPermitCommandAsync, CanExecute_AddWorkPermitCommand);
            DeleteWorkPermitCommand = new RelayCommand<WorkPermitDisplay>(Execute_DeleteWorkPermitCommand);
        }

        private string _workPermitName = null!;
        public string WorkPermitName
        {
            get => _workPermitName;
            set
            {
                SetProperty(ref _workPermitName, value);
                AddWorkPermitCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private string? _documentSeries;
        public string? DocumentSeries
        {
            get => _documentSeries;
            set
            {
                SetProperty(ref _documentSeries, value);
                AddWorkPermitCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private string _workPermitNumber = null!;
        public string WorkPermitNumber
        {
            get => _workPermitNumber;
            set
            {
                SetProperty(ref _workPermitNumber, value);
                AddWorkPermitCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private string? _protocolNumber;
        public string? ProtocolNumber
        {
            get => _protocolNumber;
            set
            {
                SetProperty(ref _protocolNumber, value);
                AddWorkPermitCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private string _specialtyName = null!;
        public string SpecialtyName
        {
            get => _specialtyName;
            set
            {
                SetProperty(ref _specialtyName, value);
                AddWorkPermitCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private string _issuingAuthority = null!;
        public string IssuingAuthority
        {
            get => _issuingAuthority;
            set
            {
                SetProperty(ref _issuingAuthority, value);
                AddWorkPermitCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private DateTime _issueDate;
        public DateTime IssueDate
        {
            get => _issueDate;
            set
            {
                SetProperty(ref _issueDate, value);
                AddWorkPermitCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private DateTime _expiryDate;
        public DateTime ExpiryDate
        {
            get => _expiryDate;
            set
            {
                SetProperty(ref _expiryDate, value);
                AddWorkPermitCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private PermitTypeDisplay _permitType = null!;
        public PermitTypeDisplay PermitType
        {
            get => _permitType;
            set
            {
                SetProperty(ref _permitType, value);
                AddWorkPermitCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private AdmissionStatusDisplay _admissionStatus = null!;
        public AdmissionStatusDisplay AdmissionStatus
        {
            get => _admissionStatus;
            set
            {
                SetProperty(ref _admissionStatus, value);
                AddWorkPermitCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        public string? FilePath
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        private WorkPermitDisplay _selectedWorkPermit = null!;
        public WorkPermitDisplay SelectedWorkPermit
        {
            get => _selectedWorkPermit;
            set => SetProperty(ref _selectedWorkPermit, value);
        }

        public ObservableCollection<string> PendingFilePaths { get; private set; } = [];

        public RelayCommand SelectMultipleCommand { get; private set; }
        private void Execute_SelectMultipleCommand()
        {
            var paths = _fileDialogService.GetFiles($"Выберите файлы: {FileDialogConsts.WORK_PERMIT}", FileFilters.Images);

            if (paths.Any())
                foreach (var path in paths)
                    PendingFilePaths.Add(path);
        }

        public ObservableCollection<WorkPermitDisplay> LocalWorkPermits { get; private init; } = [];

        public RelayCommandAsync AddWorkPermitCommandAsync { get; private set; }
        private async Task Execute_AddWorkPermitCommandAsync()
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

                    pdfBytes = await _documentConversionService.ConvertImagesToPdfAsync(images, PermitType.Name, EmployeeId!);
                }
                else
                {
                    var imageBytes = await System.IO.File.ReadAllBytesAsync(filesToProcess.First());
                    pdfBytes = await _documentConversionService.ConvertImagesToPdfAsync([imageBytes], PermitType.Name, EmployeeId!);
                }

                var fileName = $".pdf";

                LocalWorkPermits.Add
                    (
                        new WorkPermitDisplay
                            (
                                new WorkPermitResponse
                                    (
                                        string.Empty,
                                        EmployeeId,
                                        WorkPermitName,
                                        DocumentSeries,
                                        WorkPermitNumber,
                                        ProtocolNumber,
                                        SpecialtyName,
                                        IssuingAuthority,
                                        IssueDate,
                                        ExpiryDate,
                                        PermitType.Id,
                                        AdmissionStatus.Id
                                    ),
                                _permitTypes, 
                                _admissionStatuses, 
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
        private bool CanExecute_AddWorkPermitCommand()
            => AdmissionStatus != null && PermitType != null &&
            !string.IsNullOrWhiteSpace(WorkPermitName) &&
            !string.IsNullOrWhiteSpace(WorkPermitNumber) &&
            !string.IsNullOrWhiteSpace(SpecialtyName) &&
            !string.IsNullOrWhiteSpace(IssuingAuthority) &&
            IssueDate != DateTime.MinValue && 
            !string.IsNullOrWhiteSpace(IssueDate.ToString()) &&
            ExpiryDate != DateTime.MinValue &&
            !string.IsNullOrWhiteSpace(ExpiryDate.ToString());

        public RelayCommand<WorkPermitDisplay> DeleteWorkPermitCommand { get; private set; }
        private void Execute_DeleteWorkPermitCommand(WorkPermitDisplay display)
            => LocalWorkPermits.Remove(display);

        public void ClearProperty()
        {
            WorkPermitName = string.Empty;
            DocumentSeries = string.Empty;
            WorkPermitNumber = string.Empty;
            ProtocolNumber = string.Empty;
            SpecialtyName = string.Empty;
            IssuingAuthority = string.Empty;
            IssueDate = DateTime.UtcNow;
            ExpiryDate = DateTime.UtcNow;
            PermitType = null!;
            AdmissionStatus = null!;
            FilePath = string.Empty;

            PendingFilePaths.Clear();
        }
    }
}
