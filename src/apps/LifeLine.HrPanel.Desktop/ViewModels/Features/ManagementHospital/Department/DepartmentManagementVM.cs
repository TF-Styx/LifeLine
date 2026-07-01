using Shared.WPF.ViewModels.Abstract;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.ViewModels.Interfaces;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Department
{
    public sealed class DepartmentManagementVM : BaseViewModel, IChildren
    {
        private readonly ManagementHospitalStateService _stateService;

        public DepartmentListVM ListVM { get; }
        public DepartmentEditVM EditVM { get; }

        public DepartmentManagementVM(ManagementHospitalStateService stateService, DepartmentListVM listVM, DepartmentEditVM editVM)
        {
            _stateService = stateService;

            ListVM = listVM;
            EditVM = editVM;

            _stateService.BranchContextChanged += (branchId) =>
            {
                if (IsEditPanelVisible)
                    CloseEditPanel();
            };

            ListVM.RequestEditDepartment = OnEditDepartmentRequested;
            ListVM.DepartmentDeleted += OnDepartmentDeleted;

            EditVM.DepartmentSaved += OnDepartmentSaved;
            EditVM.OnCloseRequested += CloseEditPanel;
        }

        private bool _isEditPanelVisible;
        public bool IsEditPanelVisible
        {
            get => _isEditPanelVisible;
            set => SetProperty(ref _isEditPanelVisible, value);
        }

        private void OnEditDepartmentRequested(DepartmentDisplay? value)
        {
            EditVM.ClearDepartmentForm();

            if (value != null)
                EditVM.LoadDepartment(value);

            IsEditPanelVisible = true;
        }

        private void OnDepartmentSaved(DepartmentDisplay value)
        {
            IsEditPanelVisible = false;
            ListVM.UpdateDepartmentInList(value);
            EditVM.ClearDepartmentForm();
        }

        private void OnDepartmentDeleted(DepartmentDisplay value)
        {
            if (IsEditPanelVisible && EditVM.DepartmentProp != null)
                if (EditVM.DepartmentProp.DepartmentId == value.DepartmentId)
                    CloseEditPanel();
        }

        public void CloseEditPanel()
        {
            IsEditPanelVisible = false;
            EditVM.ClearDepartmentForm();
        }
    }
}
