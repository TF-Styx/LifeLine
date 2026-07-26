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

            await SubscribeToCascadeChanges();
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

        private async Task SubscribeToCascadeChanges()
        {
            if (Display is INotifyPropertyChanged npc)
            {
                npc.PropertyChanged += async (s, e) =>
                {
                    if (e.PropertyName == nameof(AssignmentContractDisplay.Hospital))
                        await OnHospitalChangedAsync();
                    else if (e.PropertyName == nameof(AssignmentContractDisplay.Branch))
                        await OnBranchChangedAsync();
                    else if (e.PropertyName == nameof(AssignmentContractDisplay.Department))
                        await OnDepartmentChangedAsync();
                };
            }
        }

        private async Task OnHospitalChangedAsync()
        {
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
            => Display = new AssignmentContractDisplay
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
                     string.Empty,
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
                 [], [], [], [], [], [], SaveStatus.Local
            );

        private bool _isLoading;

        public async Task LoadDocumentAsync(AssignmentContractDisplay display)
        {
            _isLoading = true;

            try
            {
                if (display == null)
                {
                    ClearForm();
                    return;
                }

                base.LoadDocument(display, display.AssignmentId.ToString(), display.FileKey);

                ItemVM.Clear();
                _editingId = display.AssignmentId.ToString();

                Display = new AssignmentContractDisplay
                (
                     new AssignmentResponse
                     (
                         display.Id,
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
                     _cacheService.Branches,
                     _cacheService.Departments,
                     _cacheService.Positions,
                     _cacheService.Managers,
                     _cacheService.Statuses,
                     _cacheService.EmployeeTypes,
                     SaveStatus.Local
                );

                if (display.SaveStatus == SaveStatus.DataBase && !string.IsNullOrWhiteSpace(display.FileKey))
                    await ItemVM.LoadDocumentToQueueAsync(display.FileKey);

                if (display.Hospital is not null)
                {
                    await _cascadeService.LoadBranchesByHospitalIdAsync(display.Hospital.HospitalId);

                    if (display.Branch is not null)
                    {
                        await _cascadeService.LoadDepartmentsByBranchIdAsync(display.Branch.BranchId);

                        if (display.Department is not null)
                            await _cascadeService.LoadPositionsByDepartmentIdAsync(display.Department.DepartmentId);
                    }
                }
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
                 _cacheService.Branches, 
                 _cacheService.Departments, 
                 _cacheService.Positions, 
                 _cacheService.Managers, 
                 _cacheService.Statuses, 
                 _cacheService.EmployeeTypes, 
                 SaveStatus.Local
            );

        public override void ClearForm()
        {
            Display!.Position = null!;
            Display!.Department = null!;
            Display!.Branch = null!;
            Display!.Manager = null!;
            Display!.HireDate = DateTime.Now;
            Display!.TerminationDate = DateTime.Now;
            Display!.Status = null!;

            Display!.EmployeeType = null!;
            Display!.ContractNumber = string.Empty;
            Display!.StartDate = DateTime.Now;
            Display!.EndDate = DateTime.Now;
            Display!.Salary = decimal.Zero;
            Display!.FileKey = string.Empty;

            base.ClearForm();
        }
    }
}
