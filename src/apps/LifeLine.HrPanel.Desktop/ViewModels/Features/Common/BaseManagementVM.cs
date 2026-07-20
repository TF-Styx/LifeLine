using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.Common
{
    public abstract class BaseManagementVM<TListVM, TEditVM, TDisplay> : BaseViewModel
        where TListVM : class
        where TEditVM : class
        where TDisplay : class
    {
        public TListVM ListVM { get; }
        public TEditVM EditVM { get; }

        private readonly Func<TDisplay?, Task>? _loadDocument;
        private readonly Action<TDisplay>? _updateList;
        private readonly Action? _clearEditForm;

        protected BaseManagementVM
            (
                TListVM listVM, 
                TEditVM editVM, 
                Func<TDisplay?, Task>? loadDocument, 
                Action<TDisplay>? updateList,
                Action? clearEditForm
            )
        {
            ListVM = listVM;
            EditVM = editVM;

            _loadDocument = loadDocument;
            _clearEditForm = clearEditForm;
            _updateList = updateList;
        }

        private bool _isEditPanelVisible;
        public bool IsEditPanelVisible
        {
            get => _isEditPanelVisible;
            set => SetProperty(ref _isEditPanelVisible, value);
        }

        protected async Task OnEditAsync(TDisplay? display)
        {
            _clearEditForm?.Invoke();

            if (_loadDocument != null && display != null)
                await _loadDocument(display);

            IsEditPanelVisible = true;
        }

        protected void OnSaved(TDisplay display)
        {
            _updateList?.Invoke(display);
            _clearEditForm?.Invoke();
            IsEditPanelVisible = false;
        }

        protected void OnDeleted(TDisplay display)
        {
            var prop = EditVM.GetType().GetProperty("Display");

            if (IsEditPanelVisible && prop != null)
            {
                var currentDisplay = prop.GetValue(EditVM) as TDisplay;

                if (currentDisplay != null && currentDisplay.Equals(display))
                    CloseEditPanel();
            }
        }

        public virtual void CloseEditPanel()
        {
            IsEditPanelVisible = false;
            _clearEditForm?.Invoke();
        }
    }
}
