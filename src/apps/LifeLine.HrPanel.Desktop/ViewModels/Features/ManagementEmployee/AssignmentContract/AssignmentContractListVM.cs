using LifeLine.Employee.Service.Client.Services.Employee.Assignment;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.ReferenceData;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.AssignmentContract
{
    public sealed class AssignmentContractListVM
        (
            ManagementEmployeeStateService stateService,
            IAssignmentApiServiceFactory assignmentApiServiceFactory,
            IReferenceDataCacheService cacheService
        ) : BaseDocumentListVM<AssignmentContractDisplay>(stateService)
    {
        protected override async Task LoadAsync(string employeeId)
        {
            var result = await assignmentApiServiceFactory.Create(employeeId)
                .GetAllByEmployeeId(employeeId);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }
            
            Items.Load([.. result.Value.Select(x => new AssignmentContractDisplay
            (
                x.Assignment, 
                x.Contract, 
                cacheService.Branches, 
                cacheService.Departments, 
                cacheService.Positions, 
                cacheService.Managers, 
                cacheService.Statuses, 
                cacheService.EmployeeTypes, 
                SaveStatus.DataBase
            ))], cleaning: true);
        }

        protected override async Task DeleteAsync(string employeeId, AssignmentContractDisplay display)
        {
            var result = await assignmentApiServiceFactory.Create(employeeId)
                .DeleteAssignmentContractAsync(Guid.Parse(display.AssignmentId));

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }
        }
    }
}
