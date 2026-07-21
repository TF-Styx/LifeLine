using LifeLine.File.Service.Client;
using LifeLine.HrPanel.Desktop.Services.Document.DocumentProcessing;
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
        protected readonly ManagementEmployeeStateService _stateService;
        protected readonly IFileStorageService _fileStorageService;
        protected readonly IDocumentProcessingService _documentProcessingService;

        public PendingFileItemVM ItemVM { get; }

        protected BaseDocumentEditVM
            (
                PendingFileItemVM itemVM, 
                ManagementEmployeeStateService stateServcie, 
                IFileStorageService fileStorageService,
                IDocumentProcessingService documentProcessingService
            )
        {
            ItemVM = itemVM;

            _stateService = stateServcie;
            _fileStorageService = fileStorageService;
            _documentProcessingService = documentProcessingService;

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

        protected abstract Task<(byte[] PdfBytes, string FileName)> ProcessFilesToPdfAsync(TDisplay display);
        protected abstract Task<string> UploadFileAsync(byte[] pdfBytes, string fileName, TDisplay display);
        protected abstract Task<string> CreateAsync(TDisplay display, string fileKey);
        protected abstract Task UpdateAsync(string id, TDisplay display, string fileKey);
        protected abstract TDisplay CreateDisplayFormResponse(string responseId, TDisplay display, string fileKey);
        protected abstract void InitializeNewDisplay();

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
                var (pdfBytes, fileName) = await ProcessFilesToPdfAsync(Display);

                var fileKey = await UploadFileAsync(pdfBytes, fileName, Display);

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
                var (pdfBytes, fileName) = await ProcessFilesToPdfAsync(Display);

                var newFileKey = await UploadFileAsync(pdfBytes, fileName, Display);

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
