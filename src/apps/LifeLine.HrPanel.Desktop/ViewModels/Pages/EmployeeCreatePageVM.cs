using LifeLine.Directory.Service.Client.Services.DocumentType;
using LifeLine.Employee.Service.Client.Services.Employee;
using LifeLine.Employee.Service.Client.Services.Gender;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.EmployeeService.Employee;
using Shared.Contracts.Response.DirectoryService;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Extensions;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Pages
{
    internal sealed class EmployeeCreatePageVM : BasePageViewModel, IAsyncInitializable
    {
        private readonly IEmployeeService _employeeService;
        private readonly IGenderReadOnlyService _genderReadOnlyService;
        private readonly IDocumentTypeReadOnlyService _documentTypeReadOnlyService;

        public EmployeeCreatePageVM
            (
                IEmployeeService employeeService, 
                IGenderReadOnlyService genderReadOnlyService,
                IDocumentTypeReadOnlyService documentTypeReadOnlyService
            )
        {
            _employeeService = employeeService;
            _genderReadOnlyService = genderReadOnlyService;
            _documentTypeReadOnlyService = documentTypeReadOnlyService;

            CreateEmployeeCommandAsync = new RelayCommandAsync(Execute_CreateEmployeeCommandAsync);
            AddPersonalDocumentCommand = new RelayCommand(Execute_AddPersonalDocumentCommand, CanExecute_AddPersonalDocumentCommand);
            DeletePersonalDocumentCommand = new RelayCommand<CreatedPersonalDocumentDisplay>(Execute_DeletePersonalDocumentCommand);
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            await GetAllGenderAsync();
            await GetAllDocumentTypeAsync();
        }

        #region Employees

        private string _surname = null!;
        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        private string _name = null!;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string? _patronymic;
        public string? Patronymic
        {
            get => _patronymic;
            set => SetProperty(ref _patronymic, value);
        }

        public RelayCommandAsync? CreateEmployeeCommandAsync { get; private set; }
        private async Task Execute_CreateEmployeeCommandAsync()
        {
            var result = await _employeeService.AddAsync
                (
                    new CreateEmployeeRequest
                    (
                        Surname, Name, Patronymic!, SelectedGender!.Id,
                        CreatedPersonalDocumentDisplays.Select(x => new CreateEmployeePersonalDocumentRequest(x.DocumentTypeId, x.DocumentNumber, x.DocumentSeries)).ToList()
                    )
                );

            result.Switch
                (
                    () => MessageBox.Show("Успешное добавление!"),
                    errors => MessageBox.Show(result.StringMessage)
                );
        }

        #endregion

        #region Genders

        private GenderResponse? _selectedGender;
        public GenderResponse? SelectedGender
        {
            get => _selectedGender;
            set => SetProperty(ref _selectedGender, value);
        }
        public ObservableCollection<GenderResponse> Genders { get; private init; } = [];
        private async Task GetAllGenderAsync() => Genders.Load(await _genderReadOnlyService.GetAllAsync());

        #endregion

        #region DocumentType

        private string _number;
        public string Number
        {
            get => _number;
            set
            {
                SetProperty(ref _number, value);
                AddPersonalDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        private string _series;
        public string Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        private DocumentTypeResponse _selectedDocumentType;
        public DocumentTypeResponse SelectedDocumentType
        {
            get => _selectedDocumentType;
            set
            {
                SetProperty(ref _selectedDocumentType, value);
                AddPersonalDocumentCommand?.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<DocumentTypeResponse> DocumentTypes { get; private init; } = [];
        private async Task GetAllDocumentTypeAsync() => DocumentTypes.Load(await _documentTypeReadOnlyService.GetAllAsync());

        #endregion

        #region PersonalDocument

        private CreatedPersonalDocumentDisplay _selectedCreatedPersonalDocumentDisplay;
        public CreatedPersonalDocumentDisplay SelectedCreatedPersonalDocumentDisplay
        {
            get => _selectedCreatedPersonalDocumentDisplay;
            set => SetProperty(ref _selectedCreatedPersonalDocumentDisplay, value);
        }

        public ObservableCollection<CreatedPersonalDocumentDisplay> CreatedPersonalDocumentDisplays { get; private init; } = [];

        public RelayCommand AddPersonalDocumentCommand { get; private set; }
        private void Execute_AddPersonalDocumentCommand()
        {
            var display = new CreatedPersonalDocumentDisplay
                (
                    new PersonalDocumentResponse
                        (
                            Guid.NewGuid(),
                            SelectedDocumentType.Id,
                            Number,
                            Series
                        )
                );

            CreatedPersonalDocumentDisplays.Add(display);

            display.SetDocumentTypeName(SelectedDocumentType.Name);
            display.SetDocumentNumber(Number);
            display.SetDocumentSeries(Series);
        }
        private bool CanExecute_AddPersonalDocumentCommand()
            => SelectedDocumentType != null && !string.IsNullOrWhiteSpace(Number);

        public RelayCommand<CreatedPersonalDocumentDisplay> DeletePersonalDocumentCommand { get; private set; }
        private void Execute_DeletePersonalDocumentCommand(CreatedPersonalDocumentDisplay display)
            => CreatedPersonalDocumentDisplays.Remove(display);

        #endregion
    }
}
