using LifeLine.HrPanel.Desktop.Models;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.Employee
{
    internal sealed class EmployeeManagementVM : BaseViewModel, IAsyncInitializable
    {
        public EmployeeListVM ListVM { get; }
        public EmployeeEditVM EditVM { get; }

        private readonly ManagementEmployeeStateService _stateService;

        public EmployeeManagementVM(EmployeeListVM listVM, EmployeeEditVM editVM, ManagementEmployeeStateService stateService)
        {
            ListVM = listVM;
            EditVM = editVM;

            _stateService = stateService;

            ListVM.RequestEdit = OnEditAsync;
            EditVM.OnClosed = CloseEditPanel;

            EditVM.PersonalInfo!.EmployeeSaved = async () =>
            {
                if (_stateService.EmployeeHr != null)
                {
                    await ListVM.UpdateEmployees(_stateService.EmployeeHr);
                    EditVM.ClearFormFields();
                }
            };

            EditVM.PersonalPhoto!.OnPhotoUpdated = async () =>
            {
                if (_stateService.EmployeeHr != null)
                    await ListVM.RefreshEmployeePhotoInListAsync();
            };
        }

        private bool _isEditPanelVisible;
        public bool IsEditPanelVisible
        {
            get => _isEditPanelVisible;
            set => SetProperty(ref _isEditPanelVisible, value);
        }

        private async Task OnEditAsync(EmployeeHrDisplay? display)
        {
            EditVM.ClearForm();

            if (display != null)
                await EditVM.LoadEmployeeAsync(display);

            IsEditPanelVisible = true;
        }

        private void CloseEditPanel()
        {
            EditVM.ClearForm();
            IsEditPanelVisible = false;
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            await ListVM.InitializeAsync();
        }
    }
}
