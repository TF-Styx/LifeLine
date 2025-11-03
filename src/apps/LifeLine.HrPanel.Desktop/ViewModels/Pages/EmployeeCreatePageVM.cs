using LifeLine.Directory.Service.Client.Services.DocumentType;
using LifeLine.Directory.Service.Client.Services.EducationLevel;
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
        private readonly IEducationLevelReadOnlyService _educationLevelReadOnlyService;

        public EmployeeCreatePageVM
            (
                IEmployeeService employeeService, 
                IGenderReadOnlyService genderReadOnlyService,
                IDocumentTypeReadOnlyService documentTypeReadOnlyService,
                IEducationLevelReadOnlyService educationLevelReadOnlyService
            )
        {
            _employeeService = employeeService;
            _genderReadOnlyService = genderReadOnlyService;
            _documentTypeReadOnlyService = documentTypeReadOnlyService;
            _educationLevelReadOnlyService = educationLevelReadOnlyService;

            CreateNewBaseInfoEmployee();
            CreateNewPersonalDocument();
            CreateNewEducationDocument();

            CreateEmployeeCommandAsync = new RelayCommandAsync(Execute_CreateEmployeeCommandAsync);

            AddPersonalDocumentCommand = new RelayCommand(Execute_AddPersonalDocumentCommand, CanExecute_AddPersonalDocumentCommand);
            DeletePersonalDocumentCommand = new RelayCommand<PersonalDocumentDisplay>(Execute_DeletePersonalDocumentCommand);

            AddEducationDocumentCommand = new RelayCommand(Execute_AddEducationDocumentCommand, CanExecute_AddEducationDocumentCommand);
            DeleteEducationDocumentCommand = new RelayCommand<EducationDocumentDisplay>(Execute_DeleteEducationDocumentCommand);
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            await GetAllGenderAsync();
            await GetAllDocumentTypeAsync();
            await GetAllEducationLevelAsync();
        }

        #region bool

        private bool _isUseContactInformation;
        public bool IsUseContactInformation
        {
            get => _isUseContactInformation;
            set => SetProperty(ref _isUseContactInformation, value);
        }

        private bool _isUsePersonalDocument;
        public bool IsUsePersonalDocument
        {
            get => _isUsePersonalDocument;
            set => SetProperty(ref _isUsePersonalDocument, value);
        }

        private bool _isUseEducationDocument;
        public bool IsUseEducationDocument
        {
            get => _isUseEducationDocument;
            set => SetProperty(ref _isUseEducationDocument, value);
        }

        #endregion

        #region Display

        //NewBaseInfoEmployee
        private BaseInfoEmployeeDisplay _newBaseInfoEmployee = null!;
        public BaseInfoEmployeeDisplay NewBaseInfoEmployee
        {
            get => _newBaseInfoEmployee;
            private set => SetProperty(ref _newBaseInfoEmployee, value);
        }

        private void CreateNewBaseInfoEmployee() => NewBaseInfoEmployee = new();

        //NewPersonalDocument
        private PersonalDocumentDisplay _newPersonalDocument = null!;
        public PersonalDocumentDisplay NewPersonalDocument
        {
            get => _newPersonalDocument;
            private set => SetProperty(ref _newPersonalDocument, value);
        }
        private void CreateNewPersonalDocument()
        {
            NewPersonalDocument = new(new PersonalDocumentResponse(Guid.Empty, Guid.Empty, string.Empty, string.Empty));

            NewPersonalDocument.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(NewPersonalDocument.DocumentType) || e.PropertyName == nameof(NewPersonalDocument.DocumentNumber))
                    AddPersonalDocumentCommand?.RaiseCanExecuteChanged();
            };
        }

        //NewEducationDocument
        private EducationDocumentDisplay _newEducationDocument = null!;
        public EducationDocumentDisplay NewEducationDocument
        {
            get => _newEducationDocument;
            private set => SetProperty(ref _newEducationDocument, value);
        }

        private void CreateNewEducationDocument()
        {
            NewEducationDocument = new(new EducationDocumentResponse(string.Empty, string.Empty, string.Empty, string.Empty, DateTime.UtcNow.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, "0"));

            NewEducationDocument.PropertyChanged += (s, e) => AddEducationDocumentCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Employees

        public RelayCommandAsync? CreateEmployeeCommandAsync { get; private set; }
        private async Task Execute_CreateEmployeeCommandAsync()
        {
            CreateContactInformationRequest? createContactInformation = null;

            if (IsUseContactInformation)
                createContactInformation = new CreateContactInformationRequest(NewBaseInfoEmployee.PersonalPhone, NewBaseInfoEmployee.CorporatePhone, NewBaseInfoEmployee.PersonalEmail, NewBaseInfoEmployee.CorporateEmail, NewBaseInfoEmployee.PostalCode, NewBaseInfoEmployee.Region, NewBaseInfoEmployee.City, NewBaseInfoEmployee.Street, NewBaseInfoEmployee.Building, NewBaseInfoEmployee.Apartment);

            var result = await _employeeService.AddAsync
                (
                    new CreateEmployeeRequest
                    (
                        NewBaseInfoEmployee.Surname, NewBaseInfoEmployee.Name, NewBaseInfoEmployee.Patronymic!, NewBaseInfoEmployee.Gender!.Id,
                        LocalPersonalDocuments.Select(x => new CreateEmployeePersonalDocumentRequest(x.DocumentType.Id, x.DocumentNumber, x.DocumentSeries)).ToList(),
                        createContactInformation,
                        LoacalEducationDocuments.Select(x => new CreateEducationDocumentRequest(Guid.Parse(x.EducationLevel.Id), x.DocumentType.Id, x.DocumentNumber, x.IssuedDate, x.OrganizationName, x.QualificationAwardedName, x.SpecialtyName, x.ProgramName, x.TotalHours)).ToList()
                    )
                );

            result.Switch
                (
                    () => MessageBox.Show("Успешное добавление!"),
                    errors => MessageBox.Show(result.StringMessage)
                );

            //CreateNewPersonalDocument();
        }

        #endregion

        #region Genders

        public ObservableCollection<GenderResponse> Genders { get; private init; } = [];
        private async Task GetAllGenderAsync() => Genders.Load(await _genderReadOnlyService.GetAllAsync());

        #endregion

        #region DocumentType

        public ObservableCollection<DocumentTypeResponse> DocumentTypes { get; private init; } = [];
        private async Task GetAllDocumentTypeAsync() => DocumentTypes.Load(await _documentTypeReadOnlyService.GetAllAsync());

        #endregion

        #region PersonalDocument

        private PersonalDocumentDisplay _selectedLocalPersonalDocument = null!;
        public PersonalDocumentDisplay SelectedLocalPersonalDocument
        {
            get => _selectedLocalPersonalDocument;
            set => SetProperty(ref _selectedLocalPersonalDocument, value);
        }

        public ObservableCollection<PersonalDocumentDisplay> LocalPersonalDocuments { get; private init; } = [];

        public RelayCommand? AddPersonalDocumentCommand { get; private set; }
        private void Execute_AddPersonalDocumentCommand()
        {
            LocalPersonalDocuments.Add(NewPersonalDocument);

            MessageBox.Show($"DocumentTypeId = {NewPersonalDocument.DocumentType.Id}");

            CreateNewPersonalDocument();
        }
        private bool CanExecute_AddPersonalDocumentCommand()
            => NewPersonalDocument.DocumentType != null && !string.IsNullOrWhiteSpace(NewPersonalDocument.DocumentNumber);

        public RelayCommand<PersonalDocumentDisplay>? DeletePersonalDocumentCommand { get; private set; }
        private void Execute_DeletePersonalDocumentCommand(PersonalDocumentDisplay display)
            => LocalPersonalDocuments.Remove(display);

        #endregion

        #region EducationLevel

        public ObservableCollection<EducationLevelResponse> EducationLevels { get; private init; } = [];
        private async Task GetAllEducationLevelAsync() => EducationLevels.Load(await _educationLevelReadOnlyService.GetAllAsync());

        #endregion

        #region EducationDocument

        private EducationDocumentDisplay _selectedEducationDocumentDisplay = null!;
        public EducationDocumentDisplay SelectedEducationDocumentDisplay
        {
            get => _selectedEducationDocumentDisplay;
            set => SetProperty(ref _selectedEducationDocumentDisplay, value);
        }

        public ObservableCollection<EducationDocumentDisplay> LoacalEducationDocuments { get; private init; } = [];

        public RelayCommand AddEducationDocumentCommand { get; private set; }
        private void Execute_AddEducationDocumentCommand()
        {
            LoacalEducationDocuments.Add(NewEducationDocument);

            CreateNewEducationDocument();
        }
        private bool CanExecute_AddEducationDocumentCommand()
            => NewEducationDocument.EducationLevel != null && NewEducationDocument.DocumentType != null &&
               !string.IsNullOrWhiteSpace(NewEducationDocument.DocumentNumber) && 
               !string.IsNullOrWhiteSpace(NewEducationDocument.IssuedDate.ToString()) && 
               !string.IsNullOrWhiteSpace(NewEducationDocument.OrganizationName);

        public RelayCommand<EducationDocumentDisplay>? DeleteEducationDocumentCommand { get; private set; }
        private void Execute_DeleteEducationDocumentCommand(EducationDocumentDisplay display)
            => LoacalEducationDocuments.Remove(display);

        #endregion
    }
}
