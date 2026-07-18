using LifeLine.Employee.Service.Client.Services.Employee.PersonalDocument;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.EmployeeService.PersonalDocument;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.ViewModels.Abstract;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.PersonalDocument
{
    public class PersonalDocumentEditVM : BaseViewModel
    {
        public PendingFileItemVM ItemVM { get; }

        private readonly IPersonalDocumentApiServiceFactory _personalDocumentApiServiceFactory;

        public PersonalDocumentEditVM(PendingFileItemVM itemVM, IPersonalDocumentApiServiceFactory personalDocumentApiServiceFactory)
        {
            ItemVM = itemVM;

            _personalDocumentApiServiceFactory = personalDocumentApiServiceFactory;

            NewPersonalDocumentDisplay();

            CreatePersonalDocumentCommandAsync = new RelayCommandAsync(Execute_CreatePersonalDocumentCommandAsync, CanExecute_CreatePersonalDocumentCommandAsync);
            UpdatePersonalDocumentCommandAsync = new RelayCommandAsync(Execute_UpdatePersonalDocumentCommandAsync, CanUpdatePersonalDocumentCommandAsync);

            CloseEditPanelCommand = new RelayCommand(Execute_CloseEditPanelCommand);
        }

        private string? _editingId;

        private PersonalDocumentDisplay? _display;
        public PersonalDocumentDisplay? Display
        {
            get => _display;
            set
            {
                SetProperty(ref _display, value);

                CreatePersonalDocumentCommandAsync?.RaiseCanExecuteChanged();
                UpdatePersonalDocumentCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void NewPersonalDocumentDisplay()
        {
            Display = new PersonalDocumentDisplay(new PersonalDocumentResponse(Guid.Empty, Guid.Empty, string.Empty, string.Empty, string.Empty), [], SaveStatus.Local);

            Display.PropertyChanged += (s, e) =>
            {
                CreatePersonalDocumentCommandAsync?.RaiseCanExecuteChanged();
                UpdatePersonalDocumentCommandAsync?.RaiseCanExecuteChanged();
            };
        }

        public void LoadPersonalDocument(PersonalDocumentDisplay display)
        {
            if (display == null)
            {
                NewPersonalDocumentDisplay();
                ClearPersonalDocumentForm();
                return;
            }

            _editingId = display.PersonalDocumentId.ToString();

            Display!.DocumentNumber = display.DocumentNumber;
            Display!.DocumentSeries = display.DocumentSeries;
            Display!.DocumentType = display.DocumentType;
            Display!.FileKey = display.FileKey;
        }

        public void ClearPersonalDocumentForm()
        {

            Display!.DocumentNumber = string.Empty;
            Display!.DocumentSeries = string.Empty;
            Display!.DocumentType = null!;
            Display!.FileKey = string.Empty;

            _editingId = null;
        }

        public Action<PersonalDocumentDisplay>? PersonalDocumentSaved;

        public RelayCommandAsync CreatePersonalDocumentCommandAsync { get; private set; }
        private async Task Execute_CreatePersonalDocumentCommandAsync()
        {
            var emploueeId = "123";

            var request = new CreatePersonalDocumentRequest(Guid.Parse(Display.DocumentType.Id), Display.DocumentNumber, Display.DocumentSeries, string.Empty, string.Empty);

            var result = await _personalDocumentApiServiceFactory.Create(emploueeId).AddAsync<CreatePersonalDocumentRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var newDisplay = new PersonalDocumentDisplay(new PersonalDocumentResponse(Guid.Parse(result.Value), Guid.Parse(Display.DocumentType.Id), Display.DocumentNumber, Display.DocumentSeries, string.Empty), [], SaveStatus.DataBase);

            PersonalDocumentSaved?.Invoke(newDisplay);

            NewPersonalDocumentDisplay();
            ClearPersonalDocumentForm();
            OnCloseRequested?.Invoke();
        }
        private bool CanExecute_CreatePersonalDocumentCommandAsync()
            => Display != null && Display.DocumentType != null &&
               !string.IsNullOrWhiteSpace(Display?.DocumentNumber);

        public RelayCommandAsync UpdatePersonalDocumentCommandAsync { get; private set; }
        private async Task Execute_UpdatePersonalDocumentCommandAsync()
        {
            var employeeId = "123";

            var request = new UpdatePersonalDocumentRequest(Display.DocumentType.Id, Display.DocumentNumber, Display.DocumentSeries, string.Empty, string.Empty);

            var result = await _personalDocumentApiServiceFactory.Create(employeeId).UpdatePersonalDocumentAsync(Guid.Parse(_editingId), request);
            
            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var newDisplay = new PersonalDocumentDisplay(new PersonalDocumentResponse(Guid.Parse(_editingId), Guid.Parse(Display.DocumentType.Id), Display.DocumentNumber, Display.DocumentSeries, string.Empty), [], SaveStatus.DataBase);

            PersonalDocumentSaved?.Invoke(newDisplay);

            NewPersonalDocumentDisplay();
            ClearPersonalDocumentForm();
            OnCloseRequested?.Invoke();
        }
        private bool CanUpdatePersonalDocumentCommandAsync()
            => true;

        public Action? OnCloseRequested;
        public RelayCommand CloseEditPanelCommand { get; private set; }
        private void Execute_CloseEditPanelCommand()
        {
            ClearPersonalDocumentForm();
            OnCloseRequested?.Invoke();
        }
    }
}
