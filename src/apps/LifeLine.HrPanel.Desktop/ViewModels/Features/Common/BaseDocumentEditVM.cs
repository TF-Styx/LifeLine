using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Abstract;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.Common
{
    public abstract class BaseDocumentEditVM<TDisplay, TCreateRequest, TUpdateRequest> : BaseViewModel
        where TDisplay : class
        where TCreateRequest : class
        where TUpdateRequest : class
    {
        protected readonly ManagementEmployeeStateServcie _stateService;
        protected readonly IFileStorageService _fileStorageService;

        public PendingFileItemVM ItemVM { get; }

        protected BaseDocumentEditVM(PendingFileItemVM itemVM, ManagementEmployeeStateServcie stateServcie, IFileStorageService fileStorageService)
        {
            ItemVM = itemVM;

            _stateService = stateServcie;
            _fileStorageService = fileStorageService;

            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_Command);
            UpdateCommandAsync = new RelayCommandAsync(Execute_UpdateCommandAsync, CanExecute_Command);
            CloseEditPanelCommand = new RelayCommand(Execute_CloseEditPanelCommand);
        }

        protected TDisplay? _display;
        protected string? _editingId;
        protected string? _oldFileKey;

        public Action<TDisplay>? DocumentSaved;
        public Action? OnClosed;

        public TDisplay? Display
        {
            get => _display;
            protected set
            {
                if (SetProperty(ref _display, value))
                {
                    CreateCommandAsync?.RaiseCanExecuteChanged();
                    UpdateCommandAsync?.RaiseCanExecuteChanged();
                }
            }
        }

        protected abstract Task<string> UploadFileAsync(TDisplay display);
        protected abstract Task<string> CreateAsync(TDisplay display, string fileKey);
        protected abstract Task UpdateAsync(string id, TDisplay display, string fileKey);
        protected abstract TDisplay CreateDisplayFormResponse(string responseId, TDisplay display, string fileKey);
        protected abstract void InitializeNewDisplay();

        public void LoadDocument(TDisplay display)
        {
            if (display == null)
            {
                ClearForm();
                return;
            }
        }

        public void LoadDocument(TDisplay display, string id, string? fileKey)
        {
            _editingId = id;
            _oldFileKey = fileKey;
            Display = display;
        }

        public virtual void ClearForm()
        {
            _editingId = null;
            _oldFileKey = null;
            InitializeNewDisplay();
            ItemVM.Clear();
        }

        private bool CanExecute_Command()
            => Display != null &&
               _stateService.EmployeeHr != null &&
               !string.IsNullOrWhiteSpace(_stateService.EmployeeHr.Id);

        public RelayCommandAsync CreateCommandAsync { get; private set; }
        private async Task Execute_CreateCommandAsync()
        {
            if (Display == null || _stateService.EmployeeHr == null)
            {
                MessageBox.Show("Не был выбран сотрудник!");
                return;
            }

            try
            {
                var fileKey = await UploadFileAsync(Display);
                var newId = await CreateAsync(Display, fileKey);

                var newDisplay = CreateDisplayFormResponse(newId, Display, fileKey);

                DocumentSaved?.Invoke(newDisplay);

                ClearForm();
                OnClosed?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommandAsync UpdateCommandAsync { get; private set; }
        private async Task Execute_UpdateCommandAsync()
        {
            if (Display == null || _stateService.EmployeeHr == null || string.IsNullOrWhiteSpace(_editingId))
            {
                MessageBox.Show("Не был выбран сотрудник!");
                return;
            }

            try
            {
                var newFileKey = await UploadFileAsync(Display);

                await UpdateAsync(_editingId, Display, newFileKey);

                var newDisplay = CreateDisplayFormResponse(_editingId, Display, newFileKey);

                DocumentSaved?.Invoke(newDisplay);

                ClearForm();
                OnClosed?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public RelayCommand CloseEditPanelCommand { get; private set; }
        private void Execute_CloseEditPanelCommand()
        {
            ClearForm();
            OnClosed?.Invoke();
        }
    }
}
