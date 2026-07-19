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

            ListVM.RequestEdit = OnEditRequested;
            ListVM.ItemDeleted += OnDeleted;

            EditVM.DocumentSaved += OnDocumentSaved;
            EditVM.OnClosedRequested += CloseEditPanel;
        }

        private bool _isEditPanelVisible;
        public bool IsEditPanelVisible
        {
            get => _isEditPanelVisible;
            set => SetProperty(ref _isEditPanelVisible, value);
        }

        private async Task OnEditRequested(PersonalDocumentDisplay? value)
        {
            await EditVM.LoadDocumentAsync(value!);
            IsEditPanelVisible = true;
        }

        private void OnDocumentSaved(PersonalDocumentDisplay value)
        {
            IsEditPanelVisible = false;
            ListVM.Items.Add(value);
            EditVM.ClearForm();
        }

        private void OnDeleted(PersonalDocumentDisplay value)
        {
            if (IsEditPanelVisible && EditVM.Display != null && 
                EditVM.Display.PersonalDocumentId == value.PersonalDocumentId)
                    CloseEditPanel();
        }

        public void CloseEditPanel()
        {
            IsEditPanelVisible = false;
            EditVM.ClearForm();
        }
    }
}
