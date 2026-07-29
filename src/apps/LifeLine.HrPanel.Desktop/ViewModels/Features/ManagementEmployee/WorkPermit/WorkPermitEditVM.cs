using LifeLine.Employee.Service.Client.Services.Employee.WorkPermit;
using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.Document.DocumentProcessing;
using LifeLine.HrPanel.Desktop.Services.ReferenceData;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;
using Shared.Contracts.Request.EmployeeService.WorkPermit;
using Shared.Contracts.Request.Files;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Enums;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.WorkPermit
{
    public sealed class WorkPermitEditVM : BaseDocumentEditVM<WorkPermitDisplay, CreateWorkPermitRequest, UpdateWorkPermitRequest>
    {
        private readonly IWorkPermitApiServiceFactory _workPermitApiServiceFactory;
        private readonly IReferenceDataCacheService _cacheService;

        public WorkPermitEditVM
            (
                PendingFileItemVM itemVM, 
                ManagementEmployeeStateService stateService,
                IFileStorageService fileStorageService,
                IDocumentProcessingService documentProcessingService,
                IWorkPermitApiServiceFactory workPermitApiServiceFactory,
                IReferenceDataCacheService cacheService
            ) : base(itemVM, stateService, fileStorageService, documentProcessingService)
        {
            _workPermitApiServiceFactory = workPermitApiServiceFactory;
            _cacheService = cacheService;

            InitializeNewDisplay();
        }

        public ReadOnlyObservableCollection<PermitTypeDisplay> PermitTypes => _cacheService.PermitTypes;
        public ReadOnlyObservableCollection<AdmissionStatusDisplay> AdmissionStatuses => _cacheService.AdmissionStatuses;

        protected override void InitializeNewDisplay()
            => Display = new WorkPermitDisplay
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
                    ), [], [], SaveStatus.Local
               );

        public async Task LoadDocumentAsync(WorkPermitDisplay display)
        {
            if (display == null)
            {
                ClearForm();
                return;
            }

            base.LoadDocument(display, display.WorkPermitId, display.FileKey);

            ItemVM.Clear();
            _editingId = display.WorkPermitId;

            Display = new WorkPermitDisplay
            (
                new WorkPermitResponse
                (
                    display.Id,
                    _stateService.EmployeeHr!.Id,
                    display.WorkPermitName,
                    display.DocumentSeries,
                    display.WorkPermitNumber,
                    display.ProtocolNumber,
                    display.SpecialtyName,
                    display.IssuingAuthority,
                    display.IssueDate,
                    display.ExpiryDate,
                    display.FileKey,
                    display.PermitType.Id,
                    display.AdmissionStatus.Id
                ),
                _cacheService.PermitTypes,
                _cacheService.AdmissionStatuses,
                SaveStatus.DataBase
            );

            if (display.SaveStatus == SaveStatus.DataBase && !string.IsNullOrWhiteSpace(display.FileKey))
                await ItemVM.LoadDocumentToQueueAsync(display.FileKey);
        }

        protected override async Task<(byte[] PdfBytes, string FileName)> ProcessFilesToPdfAsync(WorkPermitDisplay display)
        {
            var result = await _documentProcessingService.ProcessFilesToPdfAsync
            (
                ItemVM.PendingFilePaths,
                display.PermitType.Name,
                _stateService.EmployeeHr!.Id,
                display.WorkPermitName
            );

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return (null, null)!;
            }

            return result.Value;
        }

        protected override async Task<string> UploadFileAsync(byte[] pdfBytes, string fileName, WorkPermitDisplay display)
        {
            var employeeId = _stateService.EmployeeHr!.Id;


            var request = new UploadFileRequest
            (
                BucketName: FileConst.BUCKET_NAME,
                AdditionalName: display.PermitType.Name,
                SubFolder: FileConst.BuildEmployeeFolder
                (
                    employeeId,
                    EmployeeFolderType.WorkPermit
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

        protected override async Task<string> CreateAsync(WorkPermitDisplay display, string fileKey)
        {
            var employeeId = _stateService.EmployeeHr!.Id;

            var request = new CreateWorkPermitRequest
            (
                display.WorkPermitName,
                display.DocumentSeries,
                display.WorkPermitNumber,
                display.ProtocolNumber,
                display.SpecialtyName,
                display.IssuingAuthority,
                display.IssueDate,
                display.ExpiryDate,
                FileConst.BUCKET_NAME,
                fileKey,
                display.PermitType.Id,
                display.AdmissionStatus.Id
            );

            var result = await _workPermitApiServiceFactory.Create(employeeId)
                .AddAsync<CreateWorkPermitRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return null!;
            }

            return result.Value;
        }

        protected override async Task UpdateAsync(string id, WorkPermitDisplay display, string fileKey)
        {
            var employeeId = _stateService.EmployeeHr!.Id;

            var request = new UpdateWorkPermitRequest
                (
                    display.WorkPermitName,
                    display.DocumentSeries,
                    display.WorkPermitNumber,
                    display.ProtocolNumber,
                    display.SpecialtyName,
                    display.IssuingAuthority,
                    display.IssueDate,
                    display.ExpiryDate,
                    FileConst.BUCKET_NAME,
                    fileKey,
                    display.PermitType.Id,
                    display.AdmissionStatus.Id
                );

            var result = await _workPermitApiServiceFactory.Create(employeeId)
                .UpdateWorkPermitAsync(Guid.Parse(id), request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }
        }

        protected override WorkPermitDisplay CreateDisplayFormResponse(string responseId, WorkPermitDisplay display, string fileKey)
            => new WorkPermitDisplay
               (
                   new WorkPermitResponse
                   (
                       responseId,
                       _stateService.EmployeeHr!.Id,
                       display.WorkPermitName,
                       display.DocumentSeries,
                       display.WorkPermitNumber,
                       display.ProtocolNumber,
                       display.SpecialtyName,
                       display.IssuingAuthority,
                       display.IssueDate,
                       display.ExpiryDate,
                       fileKey,
                       display.PermitType.Id,
                       display.AdmissionStatus.Id
                   ), 
                   _cacheService.PermitTypes, 
                   _cacheService.AdmissionStatuses, 
                   SaveStatus.DataBase
               );

        public override void ClearForm()
        {
            Display!.WorkPermitName = string.Empty;
            Display!.DocumentSeries = string.Empty;
            Display!.WorkPermitNumber = string.Empty;
            Display!.ProtocolNumber = string.Empty;
            Display!.SpecialtyName = string.Empty;
            Display!.IssuingAuthority = string.Empty;
            Display!.IssueDate = DateTime.Now;
            Display!.ExpiryDate = DateTime.Now;
            Display!.FileKey = string.Empty;
            Display!.PermitType = null!;
            Display!.AdmissionStatus = null!;

            base.ClearForm();
        }
    }
}
