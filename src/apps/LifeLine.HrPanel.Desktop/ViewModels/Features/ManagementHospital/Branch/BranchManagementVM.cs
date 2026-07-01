using Shared.WPF.ViewModels.Abstract;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.ViewModels.Interfaces;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Branch
{
    public sealed class BranchManagementVM : BaseViewModel, IChildren
    {
        private readonly ManagementHospitalStateService _stateService;

        public BranchListVM ListVM { get; }
        public BranchEditVM EditVM { get; }

        public BranchManagementVM(ManagementHospitalStateService stateService, BranchListVM listVM, BranchEditVM editVM)
        {
            _stateService = stateService;

            ListVM = listVM;
            EditVM = editVM;

            _stateService.HospitalContextChanged += (hospitalId) =>
            {
                if (IsEditPanelVisible)
                    CloseEditPanel();
            };

            ListVM.RequestEditBranch = OnEditBranchRequested;
            ListVM.BranchDeleted += OnBranchDeleted;

            EditVM.BranchSaved += OnBranchSaved;
            EditVM.OnCloseRequested += CloseEditPanel;
        }

        private bool _isEditPanelVisible;
        public bool IsEditPanelVisible
        {
            get => _isEditPanelVisible;
            set => SetProperty(ref _isEditPanelVisible, value);
        }

        private void OnEditBranchRequested(BranchDisplay? value)
        {
            EditVM.ClearBranchForm();

            if (value != null)
                EditVM.LoadBranch(value);

            IsEditPanelVisible = true;
        }

        private void OnBranchSaved(BranchDisplay value)
        {
            IsEditPanelVisible = false;
            ListVM.UpdateBranchInList(value);
            EditVM.ClearBranchForm();
        }

        private void OnBranchDeleted(BranchDisplay value)
        {
            if (IsEditPanelVisible && EditVM.BranchProp != null)
                if (EditVM.BranchProp.BranchId == value.BranchId)
                    CloseEditPanel();
        }

        public void CloseEditPanel()
        {
            IsEditPanelVisible = false;
            EditVM.ClearBranchForm();
        }
    }
}
