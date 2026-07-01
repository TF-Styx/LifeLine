using Shared.WPF.ViewModels.Abstract;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.ViewModels.Interfaces;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Position
{
    public sealed class PositionManagementVM : BaseViewModel, IChildren
    {
        private readonly ManagementHospitalStateService _stateService;

        public PositionListVM ListVM { get; }
        public PositionEditVM EditVM { get; }

        public PositionManagementVM(ManagementHospitalStateService stateService, PositionListVM listVM, PositionEditVM editVM)
        {
            _stateService = stateService;

            ListVM = listVM;
            EditVM = editVM;

            _stateService.DepartmentContextChanged += (departmentId) =>
            {
                if (IsEditPanelVisible)
                    CloseEditPanel();
            };

            ListVM.RequestEditPosition = OnEditPositionRequested;
            ListVM.PositionDeleted += OnPositionDeleted;

            EditVM.PositionSaved += OnPositionSaved;
            EditVM.OnCloseRequested += CloseEditPanel;
        }

        private bool _isEditPanelVisible;
        public bool IsEditPanelVisible
        {
            get => _isEditPanelVisible;
            set => SetProperty(ref _isEditPanelVisible, value);
        }

        private void OnEditPositionRequested(PositionDisplay? value)
        {
            EditVM.ClearPositionForm();

            if (value != null)
                EditVM.LoadPosition(value);

            IsEditPanelVisible = true;
        }

        private void OnPositionSaved(PositionDisplay value)
        {
            IsEditPanelVisible = false;
            ListVM.UpdatePositionInList(value);
            EditVM.ClearPositionForm();
        }

        private void OnPositionDeleted(PositionDisplay value)
        {
            if (IsEditPanelVisible && EditVM.PositionProp != null)
                if (EditVM.PositionProp.PositionId == value.PositionId)
                    CloseEditPanel();
        }

        public void CloseEditPanel()
        {
            IsEditPanelVisible = false;
            EditVM.ClearPositionForm();
        }
    }
}
