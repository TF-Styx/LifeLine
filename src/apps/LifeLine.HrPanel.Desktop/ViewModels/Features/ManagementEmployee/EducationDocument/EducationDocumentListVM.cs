using LifeLine.Employee.Service.Client.Services.Employee.EducationDocument;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.ReferenceData;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.EducationDocument
{
    public sealed class EducationDocumentListVM
        (
            ManagementEmployeeStateService stateService,
            IEducationDocumentApiServiceFactory educationDocumentApiServiceFactory,
            IReferenceDataCacheService cacheService
        ) : BaseDocumentListVM<EducationDocumentDisplay>(stateService)
    {
        protected override async Task LoadAsync(string employeeId)
        {
            var educationDocuments = await educationDocumentApiServiceFactory.Create(employeeId)
                .GetAllByEmployeeId(employeeId);

            if (educationDocuments.IsFailure)
            {
                MessageBox.Show(educationDocuments.StringMessage);
                return;
            }

            Items.Load([.. educationDocuments.Value.Select(educationDocument => new EducationDocumentDisplay(educationDocument, cacheService.EducationLevels, cacheService.DocumentTypes, SaveStatus.DataBase))], cleaning: true);
        }

        protected override async Task DeleteAsync(string employeeId, EducationDocumentDisplay display)
        {
            var result = await educationDocumentApiServiceFactory.Create(employeeId)
                .DeleteEducationDocumentAsync(Guid.Parse(display.EducationDocumentId));

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                //throw new Exception(result.StringMessage);
                return;
            }
        }
    }
}
