using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.WorkPermit
{
    public sealed class WorkPermitManagementVM : BaseManagementVM<WorkPermitListVM, WorkPermitEditVM, WorkPermitDisplay>
    {
        public WorkPermitManagementVM
            (
                WorkPermitListVM listVM, 
                WorkPermitEditVM editVM
            ) : base
                (
                    listVM, 
                    editVM, 
                    loadDocument: async display => await editVM.LoadDocumentAsync(display!), 
                    updateList: display => listVM.UpdateInList(display!),
                    clearEditForm: () => editVM.ClearForm()
                )
        {
            listVM.RequestEdit = OnEditAsync;
            listVM.ItemDeleted += OnDeleted;

            editVM.DocumentSaved += OnSaved;
            editVM.OnClosed += CloseEditPanel;
        }
    }
}
