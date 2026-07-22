using LifeLine.Employee.Service.Client.Services.Employee.Assignment;
using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.Document.DocumentProcessing;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;
using Shared.Contracts.Request.EmployeeService.Assignment;
using Shared.Contracts.Request.Files;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Enums;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.AssignmentContract
{
    public sealed class AssignmentContractEditVM : BaseDocumentEditVM<AssignmentContractDisplay, CreateAssignmentRequest, UpdateAssignmentRequest>
    {
        private readonly IAssignmentApiServiceFactory _assignmentApiServiceFactory;

        public AssignmentContractEditVM
            (
                PendingFileItemVM itemVM,
                ManagementEmployeeStateService stateService,
                IFileStorageService fileStorageService,
                IDocumentProcessingService documentProcessingService,
                IAssignmentApiServiceFactory assignmentApiServiceFactory
            ) : base(itemVM, stateService, fileStorageService, documentProcessingService)
        {
            _assignmentApiServiceFactory = assignmentApiServiceFactory;

            InitializeNewDisplay();
        }

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

        public async Task LoadDocumentAsync(AssignmentContractDisplay display)
        {
            if (display == null)
            {
                ClearForm();
                return;
            }

            base.LoadDocument(display, display.AssignmentId.ToString(), display.FileKey);

            ItemVM.Clear();
            _editingId = display.AssignmentId.ToString();

            Display!.Position = display.Position;
            Display!.Department = display.Department;
            Display!.Branch = display.Branch;
            Display!.Manager = display.Manager;
            Display!.HireDate = display.HireDate;
            Display!.TerminationDate = display.TerminationDate;
            Display!.Status = display.Status;

            Display!.EmployeeType = display.EmployeeType;
            Display!.ContractNumber = display.ContractNumber;
            Display!.StartDate = display.StartDate;
            Display!.EndDate = display.EndDate;
            Display!.Salary = display.Salary;
            Display!.FileKey = display.FileKey;

            if (display.SaveStatus == SaveStatus.DataBase && !string.IsNullOrWhiteSpace(display.FileKey))
                await ItemVM.LoadDocumentToQueueAsync(display.FileKey, display.FileName!, display.ContentType!);
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
                     display.Status.Id
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
                 [], [], [], [], [], [], SaveStatus.Local
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
