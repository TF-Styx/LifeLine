using LifeLine.Employee.Service.Client.Services.Employee.Assignment;
using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.Document.DocumentProcessing;
using LifeLine.HrPanel.Desktop.Services.ReferenceData;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;
using Shared.Contracts.Request.EmployeeService.Assignment;
using Shared.Contracts.Request.Files;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Enums;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.AssignmentContract
{
    public sealed class AssignmentContractEditVM : BaseDocumentEditVM<AssignmentContractDisplay, CreateAssignmentRequest, UpdateAssignmentRequest>, IAsyncInitializable
    {
        private readonly IAssignmentApiServiceFactory _assignmentApiServiceFactory;
        private readonly IAssignmentCascadeService _cascadeService;
        private readonly IReferenceDataCacheService _cacheService;

        public AssignmentContractEditVM
            (
                PendingFileItemVM itemVM,
                ManagementEmployeeStateService stateService,
                IFileStorageService fileStorageService,
                IDocumentProcessingService documentProcessingService,
                IAssignmentApiServiceFactory assignmentApiServiceFactory,
                IAssignmentCascadeService cascadeService,
                IReferenceDataCacheService cacheService
            ) : base(itemVM, stateService, fileStorageService, documentProcessingService)
        {
            _assignmentApiServiceFactory = assignmentApiServiceFactory;
            _cascadeService = cascadeService;
            _cacheService = cacheService;

            InitializeNewDisplay();
        }

        public async Task InitializeAsync()
        {
            await _cascadeService.InitializeAsync();
        }

        private void SetDisplay(AssignmentContractDisplay? value)
        {
            if (_display is AssignmentContractDisplay oldDisplay)
                oldDisplay.PropertyChanged -= OnDisplayPropertyChanged;

            base.Display = value;

            if (value is AssignmentContractDisplay newDisplay)
                newDisplay.PropertyChanged += OnDisplayPropertyChanged;
        }

        private async void OnDisplayPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (_isLoading) return;

                if (e.PropertyName == nameof(AssignmentContractDisplay.Hospital))
                    await OnHospitalChangedAsync();
                else if (e.PropertyName == nameof(AssignmentContractDisplay.Branch))
                    await OnBranchChangedAsync();
                else if (e.PropertyName == nameof(AssignmentContractDisplay.Department))
                    await OnDepartmentChangedAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка каскадной загрузки: {ex.Message}");
            }
        }

        #region Списки

        public ReadOnlyObservableCollection<HospitalDisplay> Hospitals => _cascadeService.Hospitals;
        public ReadOnlyObservableCollection<BranchDisplay> Branches => _cascadeService.Branches;
        public ReadOnlyObservableCollection<DepartmentDisplay> Departments => _cascadeService.Departments;
        public ReadOnlyObservableCollection<PositionDisplay> Positions => _cascadeService.Positions;

        public ReadOnlyObservableCollection<ManagerDisplay> Managers => _cacheService.Managers;
        public ReadOnlyObservableCollection<StatusDisplay> Statuses => _cacheService.Statuses;
        public ReadOnlyObservableCollection<EmployeeTypeDisplay> EmployeeTypes => _cacheService.EmployeeTypes;

        #endregion

        #region Заполнение списков

        private async Task OnHospitalChangedAsync()
        {
            if (_isLoading)
                return;

            Display!.Branch = null!;
            Display!.Department = null!;
            Display!.Position = null!;

            if (Display!.Hospital is null)
            {
                _cascadeService.ClearHospital();
                return;
            }

            await _cascadeService.LoadBranchesByHospitalIdAsync(Display.Hospital.HospitalId);
        }

        private async Task OnBranchChangedAsync()
        {
            if (_isLoading)
                return;

            Display!.Department = null!;
            Display!.Position = null!;

            if (Display!.Branch is null)
            {
                _cascadeService.ClearBranch();
                return;
            }

            await _cascadeService.LoadDepartmentsByBranchIdAsync(Display.Branch.BranchId);
        }

        private async Task OnDepartmentChangedAsync()
        {
            if (_isLoading)
                return;

            Display!.Position = null!;

            if (Display!.Department is null)
            {
                _cascadeService.ClearDepartment();
                return;
            }

            await _cascadeService.LoadPositionsByDepartmentIdAsync(Display.Department.DepartmentId);
        }

        #endregion

        protected override void InitializeNewDisplay()
        {
            var newDisplay = new AssignmentContractDisplay
            (
                new AssignmentResponse
                (
                    string.Empty, string.Empty, string.Empty, string.Empty,
                    string.Empty, string.Empty, DateTime.Now, DateTime.Now,
                    string.Empty, string.Empty
                ),
                new ContractResponse
                (
                    string.Empty, string.Empty, string.Empty, string.Empty,
                    DateTime.Now, DateTime.Now, decimal.Zero, string.Empty
                ),
                _cascadeService.Branches,
                _cascadeService.Departments,
                _cascadeService.Positions,
                _cacheService.Managers,
                _cacheService.Statuses,
                _cacheService.EmployeeTypes,
                SaveStatus.Local
            );

            SetDisplay(newDisplay);
        }

        private bool _isLoading;

        public async Task LoadDocumentAsync(AssignmentContractDisplay display)
        {
            if (display == null)
            {
                ClearForm();
                return;
            }

            _isLoading = true;

            try
            {
                _editingId = display.AssignmentId.ToString();

                if (display.SaveStatus == SaveStatus.DataBase && !string.IsNullOrWhiteSpace(display.FileKey))
                    await ItemVM.LoadDocumentToQueueAsync(display.FileKey);

                var hospitalId = display.Hospital?.HospitalId ?? display.Branch?.HospitalId;

                if (!string.IsNullOrEmpty(hospitalId))
                {
                    await _cascadeService.LoadBranchesByHospitalIdAsync(hospitalId);

                    var branch = _cascadeService.Branches.FirstOrDefault(x => x.BranchId == display.BranchId);
                    if (branch != null)
                    {
                        await _cascadeService.LoadDepartmentsByBranchIdAsync(branch.BranchId);

                        var dept = _cascadeService.Departments.FirstOrDefault(x => x.DepartmentId == display.DepartmentId);

                        if (dept != null)
                            await _cascadeService.LoadPositionsByDepartmentIdAsync(dept.DepartmentId);
                    }
                }

                var newDisplay = new AssignmentContractDisplay(
                     display.GetUnderLineModelAssignment(),
                     display.GetUnderLineModelContract(),
                     _cascadeService.Branches, 
                     _cascadeService.Departments,
                     _cascadeService.Positions,
                     _cacheService.Managers,
                     _cacheService.Statuses,
                     _cacheService.EmployeeTypes,
                     SaveStatus.DataBase
                );

                if (!string.IsNullOrEmpty(hospitalId))
                    newDisplay.Hospital = _cascadeService.Hospitals.FirstOrDefault(h => h.HospitalId == hospitalId)!;

                SetDisplay(newDisplay);
            }
            finally
            {
                _isLoading = false;
            }
        }

        protected override async Task<(byte[] PdfBytes, string FileName)> ProcessFilesToPdfAsync(AssignmentContractDisplay display)
        {
            var result = await _documentProcessingService.ProcessFilesToPdfAsync
                (
                    ItemVM.PendingFilePaths,
                    display.Position.Name,
                    _stateService.EmployeeHr!.Id,
                    display.ContractNumber
                );

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return (null, null)!;
            }

            return result.Value;
        }

        protected override async Task<string> UploadFileAsync(byte[] pdfBytes, string fileName, AssignmentContractDisplay display)
        {
            var request = new UploadFileRequest
            (
                BucketName: FileConst.BUCKET_NAME,
                AdditionalName: display.Position.Name,
                SubFolder: FileConst.BuildEmployeeFolder
                (
                    _stateService.EmployeeHr!.Id,
                    EmployeeFolderType.Assignment
                ),
                FileBytes: pdfBytes,
                FileName: fileName,
                ContentType: display.ContentType ?? "application/pdf"
            );

            var result = await _fileStorageService.UploadFileAsync(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return null!;
            }

            return result.Value!.FileName;
        }

        protected override async Task<string> CreateAsync(AssignmentContractDisplay display, string fileKey)
        {
            var request = new CreateAssignmentRequest
                (
                    display.Position.PositionId,
                    display.Department.DepartmentId,
                    display.Branch.BranchId,
                    display.Manager != null ? display.Manager.Id : null,
                    display.HireDate,
                    display.TerminationDate,
                    display.Status.Id,
                    new CreateAssignmentContractRequest
                    (
                        display.EmployeeType.Id,
                        display.ContractNumber,
                        display.StartDate,
                        display.EndDate,
                        display.Salary,
                        FileConst.BUCKET_NAME,
                        fileKey
                    )
                );

            var result = await _assignmentApiServiceFactory.Create(_stateService.EmployeeHr!.Id)
                .AddAsync<CreateAssignmentRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return null!;
            }

            return result.Value;
        }

        protected override async Task UpdateAsync(string id, AssignmentContractDisplay display, string fileKey)
        {
            var request = new UpdateAssignmentRequest
                (
                    display.Position.PositionId,
                    display.Department.DepartmentId,
                    display.Branch.BranchId,
                    display.Manager != null ? display.Manager.Id : null,
                    display.HireDate,
                    display.TerminationDate,
                    display.Status.Id,
                    new UpdateAssignmentContractRequest
                    (
                        display.EmployeeType.Id,
                        display.ContractNumber,
                        display.StartDate,
                        display.EndDate,
                        display.Salary,
                        FileConst.BUCKET_NAME,
                        fileKey
                    )
                );

            var result = await _assignmentApiServiceFactory.Create(_stateService.EmployeeHr!.Id)
                .UpdateAssignmentAsync(Guid.Parse(id), Guid.Parse(display.ContractId),request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }
        }

        protected override AssignmentContractDisplay CreateDisplayFormResponse(string responseId, AssignmentContractDisplay display, string fileKey)
            => new AssignmentContractDisplay
            (
                 new AssignmentResponse
                 (
                     responseId,
                     _stateService.EmployeeHr!.Id,
                     display.Position.PositionId,
                     display.Department.DepartmentId,
                     display.Branch.BranchId,
                     display.Manager != null ? display.Manager.Id : null,
                     display.HireDate,
                     display.TerminationDate,
                     display.Status.Id,
                     display.ContractId
                 ),
                 new ContractResponse
                 (
                     _stateService.EmployeeHr!.Id,
                     display.ContractId,
                     display.ContractNumber,
                     display.EmployeeType.Id,
                     display.StartDate,
                     display.EndDate,
                     display.Salary,
                     display.FileKey
                 ),
                 _cascadeService.Branches, 
                 _cascadeService.Departments,
                 _cascadeService.Positions, 
                 _cacheService.Managers, 
                 _cacheService.Statuses, 
                 _cacheService.EmployeeTypes, 
                 SaveStatus.Local
            );

        public override void ClearForm()
        {
            _isLoading = true;

            try
            {
                if (Display != null)
                {
                    Display.Position = null!;
                    Display.Department = null!;
                    Display.Branch = null!;
                    Display.Hospital = null!;
                    Display.Manager = null!;
                    Display.HireDate = DateTime.Now;
                    Display.TerminationDate = DateTime.Now;
                    Display.Status = null!;
                    Display.EmployeeType = null!;
                    Display.ContractNumber = string.Empty;
                    Display.StartDate = DateTime.Now;
                    Display.EndDate = DateTime.Now;
                    Display.Salary = decimal.Zero;
                    Display.FileKey = string.Empty;
                }

                base.ClearForm();
            }
            finally
            {
                _isLoading = false;
            }
        }
    }
}
