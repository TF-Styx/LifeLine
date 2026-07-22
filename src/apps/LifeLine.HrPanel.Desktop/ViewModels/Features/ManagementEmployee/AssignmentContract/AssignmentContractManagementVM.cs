using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.ViewModels.Features.Common;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.AssignmentContract
{
    public sealed class AssignmentContractManagementVM : BaseManagementVM<AssignmentContractListVM, AssignmentContractEditVM, AssignmentContractDisplay>
    {
        public AssignmentContractManagementVM
            (
                AssignmentContractListVM listVM, 
                AssignmentContractEditVM editVM
            ) : base
                (
                    listVM, 
                    editVM, 
                    loadDocument: display => editVM.LoadDocumentAsync(display!), 
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
