using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.PersonalDocument
{
    public class PersonalDocumentManagementVM : BaseManagementVM<PersonalDocumentListVM, PersonalDocumentEditVM, PersonalDocumentDisplay>
    {
        public PersonalDocumentManagementVM
            (
                PersonalDocumentListVM listVM,
                PersonalDocumentEditVM editVM
            ) : base
                (
                    listVM,
                    editVM,
                    loadDocument: async display => await editVM.LoadDocumentAsync(display!),
                    updateList: display => listVM.UpdateInList(display!),
                    clearEditForm: () => editVM.ClearForm()
                )
        {
            ListVM.RequestEdit = OnEditAsync;
            ListVM.ItemDeleted += OnDeleted;

            EditVM.DocumentSaved += OnSaved;
            EditVM.OnClosed += CloseEditPanel;
        }
    }
}
