using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.Employee;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.ViewModels.Pages
{
    internal sealed class EmployeePageVM(EmployeeManagementVM managementVM) : BasePageViewModel, IAsyncInitializable
    {
        public EmployeeManagementVM ManagementVM { get; } = managementVM;

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize) 
                return;

            if (ManagementVM is IAsyncInitializable initializable)
                await initializable.InitializeAsync();

            IsInitialize = true;
        }
    }
}
