using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.EducationDocument
{
    public sealed class EducationDocumentManagementVM : BaseManagementVM<EducationDocumentListVM, EducationDocumentEditVM, EducationDocumentDisplay>
    {
        public EducationDocumentManagementVM
            (
                EducationDocumentListVM listVM, 
                EducationDocumentEditVM editVM
            ) : base
                (
                    listVM,
                    editVM,
                    loadDocument: async display => await editVM.LoadDocumentAsync(display!),
                    clearEditForm: () => editVM.ClearForm(),
                    updateList: display => listVM.UpdateInList(display!)
                )
        {
            ListVM.RequestEdit = OnEditAsync;
            ListVM.ItemDeleted += OnDeleted;

            EditVM.DocumentSaved += OnSaved;
            EditVM.OnClosed += CloseEditPanel;
        }
    }
}
