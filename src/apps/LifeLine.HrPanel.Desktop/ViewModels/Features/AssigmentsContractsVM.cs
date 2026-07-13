using LifeLine.Directory.Service.Client.Services.Branch;
using LifeLine.Directory.Service.Client.Services.Department;
using LifeLine.Directory.Service.Client.Services.Position.Factories;
using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.Document.DocumentProcessing;
using LifeLine.HrPanel.Desktop.Services.FilePreview;
using Shared.Contracts.Request.Files;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Constants;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using Shared.WPF.Helpers;
using Shared.WPF.Services.FileDialog;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class AssigmentsContractsVM : BaseEmployeeViewModel
    {
        private readonly IFileDialogService _fileDialogService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IFilePreviewService _filePreviewService;
        private readonly IBranchReadOnlyService _branchReadOnlyService;
        private readonly IDepartmentReadOnlyService _departmentReadOnlyService;
        private readonly IDocumentProcessingService _documentProcessingService;
        private readonly IPositionReadOnlyApiServiceFactory _positionReadOnlyApiServiceFactory;

        private readonly IReadOnlyCollection<HospitalDisplay> _hospitals;
        private readonly IReadOnlyCollection<BranchDisplay> _branches;
        private readonly IReadOnlyCollection<DepartmentDisplay> _departments;
        private readonly IReadOnlyCollection<ManagerDisplay> _managers;
        private readonly IReadOnlyCollection<StatusDisplay> _statuses;
        private readonly IReadOnlyCollection<EmployeeTypeDisplay> _employeeTypes;

        public AssigmentsContractsVM
            (
                IFileDialogService fileDialogService,
                IFileStorageService fileStorageService,
                IFilePreviewService filePreviewService,
                IBranchReadOnlyService branchReadOnlyService,
                IDepartmentReadOnlyService departmentReadOnlyService,
                IDocumentProcessingService documentProcessingService,
                IPositionReadOnlyApiServiceFactory positionReadOnlyApiServiceFactory,

                IReadOnlyCollection<HospitalDisplay> hospitals,
                IReadOnlyCollection<BranchDisplay> branches,
                IReadOnlyCollection<DepartmentDisplay> departments,
                IReadOnlyCollection<ManagerDisplay> managers,
                IReadOnlyCollection<StatusDisplay> statuses,
                IReadOnlyCollection<EmployeeTypeDisplay> employeeTypes
            )
        {
            _fileDialogService = fileDialogService;
            _fileStorageService = fileStorageService;
            _filePreviewService = filePreviewService;
            _branchReadOnlyService = branchReadOnlyService;
            _departmentReadOnlyService = departmentReadOnlyService;
            _documentProcessingService = documentProcessingService;
            _positionReadOnlyApiServiceFactory = positionReadOnlyApiServiceFactory;

            _hospitals = hospitals;
            _branches = branches;
            _departments = departments;
            _managers = managers;
            _statuses = statuses;
            _employeeTypes = employeeTypes;

            NewAssignmentContractDisplay();

            AssignmentsContractsView = CollectionViewSource.GetDefaultView(LocalAssignmentsContracts);

            AssignmentsContractsView.GroupDescriptions.Add(new PropertyGroupDescription(nameof(AssignmentContractDisplay.SaveStatus)));

            SelectMultipleCommand = new RelayCommand(Execute_SelectMultipleCommand);
            PreviewCommand = new RelayCommandAsync<PendingFileItem>(Execute_PreviewCommand);
            RemovePendingFileCommand = new RelayCommand<PendingFileItem>(Execute_RemovePendingFileCommand);
            AddAssignmentContractCommandAsync = new RelayCommandAsync(Execute_AddAssignmentContractCommandAsync, CanExecute_AddAssignmentContractCommand);

            _getAllBranchesByHospiotalIdCommandAsync = new RelayCommandAsync<HospitalDisplay>(Execute_GetAllBranchesByHospiotalIdCommandAsync);
            _getAllDepartmentsByBranchIdCommandAsync = new RelayCommandAsync<BranchDisplay>(Execute_GetAllDepartmentsByBranchIdCommandAsync);
            _getAllPositionByDepartmentIdCommandAsync = new RelayCommandAsync<DepartmentDisplay>(Execute_GetAllPositionByDepartmentIdCommandAsync);
        }

        private bool _isLoadingProgrammatically = false;

        private AssignmentContractDisplay? _display;
        public AssignmentContractDisplay? Display
        {
            get => _display;
            set
            {
                SetProperty(ref _display, value);

                AddAssignmentContractCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void NewAssignmentContractDisplay()
        {
            Display = new AssignmentContractDisplay
            (
                new AssignmentResponse
                (
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    DateTime.Now,
                    DateTime.Now,
                    string.Empty
                ),
                new ContractResponse
                (
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    DateTime.Now,
                    DateTime.Now,
                    decimal.Zero,
                    string.Empty
                ),
                _branches,
                _departments,
                Positions,
                _managers,
                _statuses,
                _employeeTypes,
                SaveStatus.Local
            );

            Display.PropertyChanged += (s, e) =>
            {
                if (_isLoadingProgrammatically)
                    return;

                if (e.PropertyName == nameof(AssignmentContractDisplay.Hospital))
                {
                    _ = LoadBranchesAsync(Display.Hospital);
                    Display.Branch = null!;
                    Display.Department = null!;
                    Display.Position = null!;
                }
                else if (e.PropertyName == nameof(AssignmentContractDisplay.Branch))
                {
                    _ = LoadDepartmentsAsync(Display.Branch);
                    Display.Department = null!;
                    Display.Position = null!;
                }
                else if (e.PropertyName == nameof(AssignmentContractDisplay.Department))
                {
                    _ = LoadPositionsAsync(Display.Department);
                    Display.Position = null!;
                }

                AddAssignmentContractCommandAsync?.RaiseCanExecuteChanged();
            };
        }

        private AssignmentContractDisplay? _selectedLocalAssignmentContract;
        public AssignmentContractDisplay? SelectedLocalAssignmentContract
        {
            get => _selectedLocalAssignmentContract;
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedLocalAssignmentContract, value);

                    _ = SetPropAsync(value);
                    _ = LoadDocumentToQueueAsync(value);
                }
            }
        }

        private async Task SetPropAsync(AssignmentContractDisplay value)
        {
            _isLoadingProgrammatically = true;

            try
            {
                Display?.Hospital = _hospitals.FirstOrDefault(x => x.HospitalId == value.Branch.HospitalId)!;
                await LoadBranchesAsync(Display!.Hospital);

                Display?.Branch = Branches.FirstOrDefault(x => x.BranchId == value.Branch.BranchId)!;
                await LoadDepartmentsAsync(Display!.Branch);

                Display?.Department = Departments.FirstOrDefault(x => x.DepartmentId == value.Department.DepartmentId)!;
                await LoadPositionsAsync(Display!.Department);

                Display?.Position = Positions.FirstOrDefault(x => x.PositionId == value.Position.PositionId)!;

                Display?.Manager = value.Manager;
                Display?.HireDate = value.HireDate;
                Display?.TerminationDate = value.TerminationDate;
                Display?.Status = value.Status;

                Display?.EmployeeType = value.EmployeeType;
                Display?.ContractNumber = value.ContractNumber;
                Display?.StartDate = value.StartDate;
                Display?.EndDate = value.EndDate;
                Display?.Salary = value.Salary;
            }
            finally
            {
                _isLoadingProgrammatically = false;
            }
        }

        private async Task LoadDocumentToQueueAsync(AssignmentContractDisplay document)
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
                Debug.WriteLine($"[AssigmentsContractsVM] [Execute_PreviewCommand] item пуст!");
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

        public ObservableCollection<BranchDisplay> Branches { get; private set; } = [];
        private async Task LoadBranchesAsync(HospitalDisplay display)
        {
            if (display == null || string.IsNullOrWhiteSpace(display.HospitalId))
            {
                Branches.Clear();
                return;
            }

            var branchesResult = await _branchReadOnlyService.GetAllByHospitalIdAsync(display.HospitalId);

            if (branchesResult.IsFailure) 
                return;

            Branches.Load([.. branchesResult.Value.Select(branch => new BranchDisplay(branch))], cleaning: true);
        }
        private RelayCommandAsync<HospitalDisplay> _getAllBranchesByHospiotalIdCommandAsync;
        private async Task Execute_GetAllBranchesByHospiotalIdCommandAsync(HospitalDisplay display) => await LoadBranchesAsync(display);

        public ObservableCollection<DepartmentDisplay> Departments { get; private set; } = [];
        private async Task LoadDepartmentsAsync(BranchDisplay display)
        {
            if (display == null || string.IsNullOrWhiteSpace(display.BranchId))
            {
                Departments.Clear();
                return;
            }

            var departmentsResult = await _departmentReadOnlyService.GetAllByBranchIdAsync(display.BranchId);

            if (departmentsResult.IsFailure) 
                return;

            Departments.Load([.. departmentsResult.Value.Select(department => new DepartmentDisplay(department))], cleaning: true);
        }
        private RelayCommandAsync<BranchDisplay> _getAllDepartmentsByBranchIdCommandAsync;
        private async Task Execute_GetAllDepartmentsByBranchIdCommandAsync(BranchDisplay display) => await LoadDepartmentsAsync(display);

        public ObservableCollection<PositionDisplay> Positions { get; private init; } = [];
        private async Task LoadPositionsAsync(DepartmentDisplay display)
        {
            if (display == null || display.DepartmentId == string.Empty)
            {
                Positions.Clear();
                return;
            }

            var positions = await _positionReadOnlyApiServiceFactory.Create(display.DepartmentId.ToString()).GetAllAsync();

            Positions.Load([.. positions.Select(position => new PositionDisplay(position))], cleaning: true);
        }
        private RelayCommandAsync<DepartmentDisplay> _getAllPositionByDepartmentIdCommandAsync;
        private async Task Execute_GetAllPositionByDepartmentIdCommandAsync(DepartmentDisplay display) => await LoadPositionsAsync(display);

        public ObservableCollection<AssignmentContractDisplay> LocalAssignmentsContracts { get; private init; } = [];
        public ICollectionView AssignmentsContractsView { get; private init; } = null!;

        public RelayCommandAsync AddAssignmentContractCommandAsync { get; private set; }
        private async Task Execute_AddAssignmentContractCommandAsync()
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
                    Display.Position.Name,
                    EmployeeId!,
                    Display.ContractNumber
                );

            if (processResult.IsFailure)
            {
                MessageBox.Show(processResult.StringMessage);
                return;
            }

            var (pdfBytes, fileName) = processResult.Value;

            LocalAssignmentsContracts.Add
            (
                new AssignmentContractDisplay
                    (
                        new AssignmentResponse
                            (
                                string.Empty,
                                EmployeeId!,
                                Display.Position.PositionId,
                                Display.Department.DepartmentId,
                                Display.Branch.BranchId,
                                Display.Manager?.Id,
                                Display.HireDate,
                                Display.TerminationDate,
                                Display.Status.Id
                            ),
                        new ContractResponse
                            (
                                EmployeeId!,
                                string.Empty,
                                Display.ContractNumber,
                                Display.EmployeeType.Id,
                                Display.StartDate,
                                Display.EndDate,
                                Display.Salary,
                                null
                            ),
                        _branches,
                        _departments,
                        Positions,
                        _managers,
                        _statuses,
                        _employeeTypes,
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
        private bool CanExecute_AddAssignmentContractCommand()
            => Display?.Hospital != null && Display?.Branch != null && 
               Display?.Department != null && Display?.Position != null &&
               Display?.EmployeeType != null && Display?.Status != null &&
               !string.IsNullOrWhiteSpace(Display?.HireDate.ToString()) &&
               !string.IsNullOrWhiteSpace(Display?.ContractNumber) &&
               !string.IsNullOrWhiteSpace(Display?.StartDate.ToString()) &&
               !string.IsNullOrWhiteSpace(Display?.EndDate.ToString()) &&
               !string.IsNullOrWhiteSpace(Display?.Salary.ToString());

        public void ClearProperty()
        {
            Display.Hospital = null!;
            Display.Branch = null!;
            Display.Department = null!;
            Display.Position = null!;
            Display.Manager = null;
            Display.HireDate = DateTime.Now;
            Display.TerminationDate = DateTime.Now;
            Display.Status = null!;

            Display.EmployeeType = null!;
            Display.ContractNumber = string.Empty;
            Display.StartDate = DateTime.Now;
            Display.EndDate = DateTime.Now;
            Display.Salary = decimal.Zero;

            PendingFilePaths.Clear();
            SelectedLocalAssignmentContract = null!;
        }

        private void UpdateIndexes()
        {
            for (int i = 0; i < PendingFilePaths.Count; i++)
                PendingFilePaths[i].Index = i + 1;
        }
    }
}
