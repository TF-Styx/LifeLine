using LifeLine.HrPanel.Desktop.Models;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.PersonalDocument
{
    public class PersonalDocumentManagementVM : BaseViewModel
    {
        public PersonalDocumentListVM ListVM { get; }
        public PersonalDocumentEditVM EditVM { get; }

        public PersonalDocumentManagementVM(PersonalDocumentListVM listVM, PersonalDocumentEditVM editVM)
        {
            ListVM = listVM;
            EditVM = editVM;

            //ListVM.RequestEditBranch = OnEditBranchRequested;
            //ListVM.BranchDeleted += OnBranchDeleted;

            EditVM.PersonalDocumentSaved+= OnPersonalDocumentSaved;
            EditVM.OnCloseRequested += CloseEditPanel;
        }

        private bool _isEditPanelVisiblel;
        public bool IsEditPanelVisible
        {
            get => _isEditPanelVisiblel;
            set => SetProperty(ref _isEditPanelVisiblel, value);
        }

        private void OnEditPersonalDocumentRequested(PersonalDocumentDisplay? value)
        {
            EditVM.ClearPersonalDocumentForm();

            if (value != null)
                EditVM.LoadPersonalDocument(value);

            IsEditPanelVisible = true;
        }

        private void OnPersonalDocumentSaved(PersonalDocumentDisplay value)
        {
            IsEditPanelVisible = false;
            //ListVM.UpdatePersonalDocumentInList(value);
            EditVM.ClearPersonalDocumentForm();
        }

        private void OnBranchDeleted(PersonalDocumentDisplay value)
        {
            if (IsEditPanelVisible && EditVM.Display != null)
                if (EditVM.Display.PersonalDocumentId == value.PersonalDocumentId)
                    CloseEditPanel();
        }

        public void CloseEditPanel()
        {
            IsEditPanelVisible = false;
            EditVM.ClearPersonalDocumentForm();
        }
    }
}
