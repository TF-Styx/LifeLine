using Shared.WPF.ViewModels.Abstract;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.ViewModels.Interfaces;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Hospital
{
    public sealed class HospitalManagementVM : BaseViewModel, IChildren
    {
        public HospitalListVM ListVM { get; }
        public HospitalEditVM EditVM { get; }

        public HospitalManagementVM(HospitalListVM listVM, HospitalEditVM editVM)
        {
            ListVM = listVM;
            EditVM = editVM;

            ListVM.RequestEditHospital += OnEditHospitalRequested;
            ListVM.HospitalDeleted += OnHospitalDeleted;

            EditVM.HospitalSaved += OnHospitalSaved;
            EditVM.OnCloseRequested += CloseEditPanel;
        }

        private bool _isEditPanelVisible;
        public bool IsEditPanelVisible
        {
            get => _isEditPanelVisible;
            set => SetProperty(ref _isEditPanelVisible, value);
        }

        private void OnEditHospitalRequested(HospitalDisplay? value)
        {
            EditVM.ClearHospitalForm();

            if (value != null)
                EditVM.LoadHospital(value);

            IsEditPanelVisible = true;
        }

        private void OnHospitalSaved(HospitalDisplay value)
        {
            IsEditPanelVisible = false;
            ListVM.UpdateHospitalInList(value);
            EditVM.ClearHospitalForm();
        }

        private void OnHospitalDeleted(HospitalDisplay value)
        {
            if (IsEditPanelVisible && EditVM.HospitalProp != null)
                if (EditVM.HospitalProp.HospitalId == value.HospitalId)
                    CloseEditPanel();
        }

        private void CloseEditPanel()
        {
            IsEditPanelVisible = false;
            EditVM.ClearHospitalForm();
        }
    }
}
