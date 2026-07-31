using LifeLine.Employee.Service.Client.Services.Employee.EducationDocument;
using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.Document.DocumentProcessing;
using LifeLine.HrPanel.Desktop.Services.ReferenceData;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;
using Shared.Contracts.Request.EmployeeService.EducationDocument;
using Shared.Contracts.Request.Files;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Enums;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.EducationDocument
{
    public sealed class EducationDocumentEditVM : BaseDocumentEditVM<EducationDocumentDisplay, CreateEducationDocumentRequest, UpdateEducationDocumentRequest>
    {
        private readonly IEducationDocumentApiServiceFactory _educationDocumentApiServiceFactory;
        private readonly IReferenceDataCacheService _cacheService;

        public EducationDocumentEditVM
            (
                PendingFileItemVM itemVM,
                ManagementEmployeeStateService stateService,
                IFileStorageService fileStorageService,
                IDocumentProcessingService documentProcessingService,
                IEducationDocumentApiServiceFactory educationDocumentApiServiceFactory,
                IReferenceDataCacheService cacheService
            ) : base(itemVM, stateService, fileStorageService, documentProcessingService)
        {
            _educationDocumentApiServiceFactory = educationDocumentApiServiceFactory;
            _cacheService = cacheService;

            InitializeNewDisplay();
        }

        public ReadOnlyObservableCollection<DocumentTypeDisplay> DocumentTypes => _cacheService.DocumentTypes;
        public ReadOnlyObservableCollection<EducationLevelDisplay> EducationLevels => _cacheService.EducationLevels;

        protected override void InitializeNewDisplay()
            => Display = new EducationDocumentDisplay
               (
                   new EducationDocumentResponse
                   (
                       string.Empty,
                       string.Empty,
                       string.Empty,
                       string.Empty,
                       string.Empty,
                       DateTime.Now.ToString(),
                       string.Empty,
                       string.Empty,
                       string.Empty,
                       string.Empty,
                       string.Empty,
                       string.Empty
                   ), [], [], SaveStatus.Local
               );

        public async Task LoadDocumentAsync(EducationDocumentDisplay display)
        {
            if (display == null)
            {
                ClearForm();
                return;
            }

            base.LoadDocument(display, display.EducationDocumentId, display.FileKey);

            ItemVM.Clear();
            _editingId = display.EducationDocumentId;

            Display = new EducationDocumentDisplay
            (
                new EducationDocumentResponse
                (
                    display.Id,
                    _stateService.EmployeeHr!.Id,
                    display.EducationLevel.Id,
                    display.DocumentType.Id,
                    display.DocumentNumber,
                    display.IssuedDate.ToString(),
                    display.OrganizationName,
                    display.QualificationAwardedName,
                    display.SpecialtyName,
                    display.ProgramName,
                    display.TotalHours.ToString(),
                    display.FileKey
                ),
                _cacheService.EducationLevels,
                _cacheService.DocumentTypes,
                SaveStatus.Local
            );

            if (display.SaveStatus == SaveStatus.DataBase && !string.IsNullOrWhiteSpace(display.FileKey))
                await ItemVM.LoadDocumentToQueueAsync(display.FileKey);
        }

        protected override async Task<(byte[] PdfBytes, string FileName)> ProcessFilesToPdfAsync(EducationDocumentDisplay display)
        {
            var result = await _documentProcessingService.ProcessFilesToPdfAsync
            (
                ItemVM.PendingFilePaths,
                display.DocumentType.Name,
                _stateService.EmployeeHr!.Id,
                display.DocumentNumber
            );

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return (null, null)!;
            }

            return result.Value;
        }

        protected override async Task<string> UploadFileAsync(byte[] pdfBytes, string fileName, EducationDocumentDisplay display)
        {
            var employeeId = _stateService.EmployeeHr!.Id;

            var request = new UploadFileRequest
            (
                BucketName: FileConst.BUCKET_NAME,
                AdditionalName: display.DocumentType.Name,
                SubFolder: FileConst.BuildEmployeeFolder
                (
                    employeeId,
                    EmployeeFolderType.EducationDocument
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

        protected override async Task<string> CreateAsync(EducationDocumentDisplay display, string fileKey)
        {
            var employeeId = _stateService.EmployeeHr!.Id;

            var request = new CreateEducationDocumentRequest
            (
                Guid.Parse(display.EducationLevel.Id),
                Guid.Parse(display.DocumentType.Id),
                display.DocumentNumber,
                display.IssuedDate,
                display.OrganizationName,
                display.QualificationAwardedName,
                display.SpecialtyName,
                display.ProgramName,
                display.TotalHours,
                FileConst.BUCKET_NAME,
                fileKey
            );

            var result = await _educationDocumentApiServiceFactory.Create(employeeId)
                .AddAsync<CreateEducationDocumentRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return null!;
            }

            return result.Value;
        }

        protected override async Task UpdateAsync(string id, EducationDocumentDisplay display, string fileKey)
        {
            var employeeId = _stateService.EmployeeHr!.Id;

            var request = new UpdateEducationDocumentRequest
            (
                display.EducationLevel.Id,
                display.DocumentType.Id,
                display.DocumentNumber,
                display.IssuedDate,
                display.OrganizationName,
                display.QualificationAwardedName,
                display.SpecialtyName,
                display.ProgramName,
                display.TotalHours,
                FileConst.BUCKET_NAME,
                fileKey
            );

            var result = await _educationDocumentApiServiceFactory.Create(employeeId)
                .UpdateEducationDocumentAsync(Guid.Parse(id), request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }
        }

        protected override EducationDocumentDisplay CreateDisplayFormResponse(string responseId, EducationDocumentDisplay display, string fileKey) 
            => new EducationDocumentDisplay
               (
                   new EducationDocumentResponse
                   (
                       responseId,
                       _stateService.EmployeeHr!.Id,
                       display.EducationLevel.Id,
                       display.DocumentType.Id,
                       display.DocumentNumber,
                       display.IssuedDate.ToString(),
                       display.OrganizationName,
                       display.QualificationAwardedName,
                       display.SpecialtyName,
                       display.ProgramName,
                       display.TotalHours.ToString(),
                       fileKey
                   ), 
                   _cacheService.EducationLevels, 
                   _cacheService.DocumentTypes, 
                   SaveStatus.Local
               );

        public override void ClearForm()
        {
            Display!.EducationLevel = null!;
            Display!.DocumentType = null!;
            Display!.DocumentNumber = string.Empty;
            Display!.IssuedDate = DateTime.Now;
            Display!.OrganizationName = string.Empty;
            Display!.QualificationAwardedName = string.Empty;
            Display!.SpecialtyName = string.Empty;
            Display!.ProgramName = string.Empty;
            Display!.TotalHours = TimeSpan.Zero;
            Display!.FileKey = string.Empty;

            base.ClearForm();
        }
    }
}
