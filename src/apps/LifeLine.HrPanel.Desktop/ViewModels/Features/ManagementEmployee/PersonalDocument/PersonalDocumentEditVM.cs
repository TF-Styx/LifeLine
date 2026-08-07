using LifeLine.Employee.Service.Client.Services.Employee.PersonalDocument;
using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.Document.DocumentProcessing;
using LifeLine.HrPanel.Desktop.Services.ReferenceData;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;
using Shared.Contracts.Request.EmployeeService.PersonalDocument;
using Shared.Contracts.Request.Files;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Enums;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.PersonalDocument
{
    public class PersonalDocumentEditVM : BaseDocumentEditVM<PersonalDocumentDisplay, CreatePersonalDocumentRequest, UpdatePersonalDocumentRequest>
    {
        private readonly IPersonalDocumentApiServiceFactory _personalDocumentApiServiceFactory;
        private readonly IReferenceDataCacheService _cacheService;

        public PersonalDocumentEditVM
            (
                PendingFileItemVM itemVM, 
                ManagementEmployeeStateService stateService,
                IFileStorageService fileStorageService,
                IDocumentProcessingService documentProcessingService,
                IPersonalDocumentApiServiceFactory personalDocumentApiServiceFactory,
                IReferenceDataCacheService cacheService
            ) : base(itemVM, stateService, fileStorageService, documentProcessingService)
        {
            _personalDocumentApiServiceFactory = personalDocumentApiServiceFactory;
            _cacheService = cacheService;

            InitializeNewDisplay();
        }

        public ReadOnlyObservableCollection<DocumentTypeDisplay> DocumentTypes => _cacheService.DocumentTypes;

        protected override void InitializeNewDisplay()
            => Display = new PersonalDocumentDisplay
                (
                    new PersonalDocumentResponse
                    (
                        Guid.Empty, 
                        Guid.Empty, 
                        string.Empty, 
                        string.Empty, 
                        string.Empty
                    ), [], SaveStatus.Local
                );

        public async Task LoadDocumentAsync(PersonalDocumentDisplay display)
        {
            if (display == null)
            {
                ClearForm();
                return;
            }

            base.LoadDocument(display, display.PersonalDocumentId.ToString(), display.FileKey);

            ItemVM.Clear();
            _editingId = display.PersonalDocumentId.ToString();

            Display = new PersonalDocumentDisplay
            (
                new PersonalDocumentResponse
                (
                    display.PersonalDocumentId,
                    display.DocumentType?.Id != null ? Guid.Parse(display.DocumentType.Id) : Guid.Empty,
                    display.DocumentNumber,
                    display.DocumentSeries,
                    display.FileKey
                ),
                _cacheService.DocumentTypes,
                display.SaveStatus
            );

            if (display.SaveStatus == SaveStatus.DataBase && !string.IsNullOrWhiteSpace(display.FileKey))
                await ItemVM.LoadDocumentToQueueAsync(display.FileKey);
        }

        protected override async Task<(byte[] PdfBytes, string FileName)> ProcessFilesToPdfAsync(PersonalDocumentDisplay display)
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

        protected override async Task<string> UploadFileAsync(byte[] pdfBytes, string fileName, PersonalDocumentDisplay display)
        {
            var employeeId = _stateService.EmployeeHr!.Id;

            var request = new UploadFileRequest
            (
                BucketName: FileConst.BUCKET_NAME,
                AdditionalName: display.DocumentType.Name,
                SubFolder: FileConst.BuildEmployeeFolder
                (
                    employeeId,
                    EmployeeFolderType.PersonalDocument
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

        protected override async Task<string> CreateAsync(PersonalDocumentDisplay display, string fileKey)
        {
            var employeeId = _stateService.EmployeeHr!.Id;

            var request = new CreatePersonalDocumentRequest
            (
                Guid.Parse(display.DocumentType.Id),
                display.DocumentNumber,
                display.DocumentSeries,
                FileConst.BUCKET_NAME,
                fileKey
            );

            var result = await _personalDocumentApiServiceFactory.Create(employeeId)
                .AddAsync<CreatePersonalDocumentRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return null!;
            }

            return result.Value;
        }

        protected override async Task UpdateAsync(string id, PersonalDocumentDisplay display, string fileKey)
        {
            var employeeId = _stateService.EmployeeHr!.Id;

            var request = new UpdatePersonalDocumentRequest
            (
                display.DocumentType.Id,
                display.DocumentNumber,
                display.DocumentSeries,
                FileConst.BUCKET_NAME,
                fileKey
            );

            var result = await _personalDocumentApiServiceFactory.Create(employeeId)
                .UpdatePersonalDocumentAsync(Guid.Parse(id), request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }
        }

        protected override PersonalDocumentDisplay CreateDisplayFormResponse(string responseId, PersonalDocumentDisplay display, string fileKey)
            => new PersonalDocumentDisplay
               (
                   new PersonalDocumentResponse
                   (
                       Guid.Parse(responseId),
                       Guid.Parse(display.DocumentType.Id),
                       display.DocumentNumber,
                       display.DocumentSeries,
                       fileKey
                   ),
                   _cacheService.DocumentTypes,
                   SaveStatus.DataBase
               );

        public override void ClearForm()
        {
            Display!.DocumentNumber = string.Empty;
            Display!.DocumentSeries = string.Empty;
            Display!.DocumentType = null!;
            Display!.FileKey = string.Empty;

            base.ClearForm();
        }
    }
}
