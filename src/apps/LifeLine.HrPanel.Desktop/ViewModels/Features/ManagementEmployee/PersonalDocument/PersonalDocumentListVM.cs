using LifeLine.Employee.Service.Client.Services.Employee.PersonalDocument;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.PersonalDocument
{
    public class PersonalDocumentListVM
        (
            ManagementEmployeeStateService stateService,
            IPersonalDocumentApiServiceFactory personalDocumentApiServiceFactory
        ) : BaseDocumentListVM<PersonalDocumentDisplay>(stateService)
    {
        protected override async Task LoadAsync(string employeeId)
        {
            var personalDocuments = await personalDocumentApiServiceFactory.Create(employeeId)
                .GetAllByEmployeeId(employeeId);

            if (personalDocuments.IsFailure)
            {
                MessageBox.Show(personalDocuments.StringMessage);
                return;
            }

            Items.Load([.. personalDocuments.Value.Select(personalDocument => new PersonalDocumentDisplay(personalDocument, [], SaveStatus.DataBase))], cleaning: true);
        }

        protected override async Task DeleteAsync(string employeeId, PersonalDocumentDisplay display)
        {
            var result = await personalDocumentApiServiceFactory.Create(employeeId)
                .DeletePersonalDocumentAsync(display.PersonalDocumentId);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                throw new Exception(result.StringMessage);
            }
        }
    }
}
