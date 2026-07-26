using LifeLine.Employee.Service.Client.Services.Employee.WorkPermit;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.ReferenceData;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using System.Windows;
using Terminex.Common.Results;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.WorkPermit
{
    public sealed class WorkPermitListVM
        (
            ManagementEmployeeStateService stateService,
            IWorkPermitApiServiceFactory workPermitApiServiceFactory,
            IReferenceDataCacheService cacheService
        ): BaseDocumentListVM<WorkPermitDisplay>(stateService)
    {
        protected override async Task LoadAsync(string employeeId)
        {
            var workPermits = await workPermitApiServiceFactory.Create(employeeId)
                .GetAllByEmployeeId(employeeId);

            if (workPermits.IsFailure)
            {
                MessageBox.Show(workPermits.StringMessage);
                return;
            }

            Items.Load([.. workPermits.Value.Select(workPermit => new WorkPermitDisplay(workPermit, cacheService.PermitTypes, cacheService.AdmissionStatuses, SaveStatus.DataBase))], cleaning: true);
        }

        protected override async Task DeleteAsync(string employeeId, WorkPermitDisplay display)
        {
            var result = await workPermitApiServiceFactory.Create(employeeId)
                .DeleteWorkPermitAsync(Guid.Parse(display.WorkPermitId));

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                throw new Exception(result.StringMessage);
            }
        }
    }
}
