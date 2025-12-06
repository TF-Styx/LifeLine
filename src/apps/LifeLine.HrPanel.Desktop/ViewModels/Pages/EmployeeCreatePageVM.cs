using LifeLine.Directory.Service.Client.Services.AdmissionStatus;
using LifeLine.Directory.Service.Client.Services.Department;
using LifeLine.Directory.Service.Client.Services.DocumentType;
using LifeLine.Directory.Service.Client.Services.EducationLevel;
using LifeLine.Directory.Service.Client.Services.PermitType;
using LifeLine.Directory.Service.Client.Services.Position.Factories;
using LifeLine.Directory.Service.Client.Services.Status;
using LifeLine.Employee.Service.Client.Services.Employee;
using LifeLine.Employee.Service.Client.Services.EmployeeType;
using LifeLine.Employee.Service.Client.Services.Gender;
using LifeLine.Employee.Service.Client.Services.Specialty;
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
        private readonly IStatusReadOnlyService _statusReadOnlyService;
        private readonly ISpecialtyReadOnlyService _specialtyReadOnlyService;
        private readonly IPermitTypeReadOnlyService _permitTypeReadOnlyService;
        private readonly IDepartmentReadOnlyService _departmentReadOnlyService;
        private readonly IDocumentTypeReadOnlyService _documentTypeReadOnlyService;
        private readonly IEmployeeTypeReadOnlyService _employeeTypeReadOnlyService;
        private readonly IEducationLevelReadOnlyService _educationLevelReadOnlyService;
        private readonly IAdmissionStatusReadOnlyService _admissionStatusReadOnlyService;
        private readonly IPositionReadOnlyApiServiceFactory _positionReadOnlyApiServiceFactory;

        public EmployeeCreatePageVM
            (
                IEmployeeService employeeService, 
                IGenderReadOnlyService genderReadOnlyService,
                IStatusReadOnlyService statusReadOnlyService,
                ISpecialtyReadOnlyService specialtyReadOnlyService,
                IPermitTypeReadOnlyService permitTypeReadOnlyService,
                IDepartmentReadOnlyService departmentReadOnlyService,
                IDocumentTypeReadOnlyService documentTypeReadOnlyService,
                IEmployeeTypeReadOnlyService employeeTypeReadOnlyService,
                IEducationLevelReadOnlyService educationLevelReadOnlyService,
                IAdmissionStatusReadOnlyService admissionStatusReadOnlyService,
                IPositionReadOnlyApiServiceFactory positionReadOnlyApiServiceFactory
            )
        {
            _employeeService = employeeService;
            _genderReadOnlyService = genderReadOnlyService;
            _statusReadOnlyService = statusReadOnlyService;
            _specialtyReadOnlyService = specialtyReadOnlyService;
            _permitTypeReadOnlyService = permitTypeReadOnlyService;
            _departmentReadOnlyService = departmentReadOnlyService;
            _documentTypeReadOnlyService = documentTypeReadOnlyService;
            _employeeTypeReadOnlyService = employeeTypeReadOnlyService;
            _educationLevelReadOnlyService = educationLevelReadOnlyService;
            _admissionStatusReadOnlyService = admissionStatusReadOnlyService;
            _positionReadOnlyApiServiceFactory = positionReadOnlyApiServiceFactory;

            CreateNewWorkPermit();
            CreateNewBaseInfoEmployee();
            CreateNewPersonalDocument();
            CreateNewEducationDocument();
            CreateNewAssignmentContract();

            CreateEmployeeCommandAsync = new RelayCommandAsync(Execute_CreateEmployeeCommandAsync);

            AddPersonalDocumentCommand = new RelayCommand(Execute_AddPersonalDocumentCommand, CanExecute_AddPersonalDocumentCommand);
            DeletePersonalDocumentCommand = new RelayCommand<PersonalDocumentDisplay>(Execute_DeletePersonalDocumentCommand);

            AddEducationDocumentCommand = new RelayCommand(Execute_AddEducationDocumentCommand, CanExecute_AddEducationDocumentCommand);
            DeleteEducationDocumentCommand = new RelayCommand<EducationDocumentDisplay>(Execute_DeleteEducationDocumentCommand);

            AddWorkPermitCommand = new RelayCommand(Execute_AddWorkPermitCommand, CanExecute_AddWorkPermitCommand);
            DeleteWorkPermitCommand = new RelayCommand<WorkPermitDisplay>(Execute_DeleteWorkPermitCommand);

            AddEmployeeSpecialtyCommand = new RelayCommand(Execute_AddEmployeeSpecialtyCommand, CanExecute_AddEmployeeSpecialtyCommand);
            DeleteEmployeeSpecialtyCommand = new RelayCommand<SpecialtyResponse>(Execute_DeleteEmployeeSpecialtyCommand);

            AddAssignmentContractCommand = new RelayCommand(Execute_AddAssignmentContractCommand, CanExecute_AddAssignmentContractCommand);
            DeleteAssignmentContractCommand = new RelayCommand<AssignmentContractDisplay>(Execute_DeleteAssignmentContractCommand);
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            await GetAllSpecialty();
            await GetAllGenderAsync();
            await GetAllStatusAsync();
            await GetAllManagerAsync();
            await GetAllPermitTypeAsync();
            await GetAllDepartmentAsync();
            await GetAllDocumentTypeAsync();
            await GetAllEmployeeTypeAsync();
            await GetAllEducationLevelAsync();
            await GetAllAdmissionStatusAsync();

            IsInitialize = true;
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

        private bool _isUseWorkPermit;
        public bool IsUseWorkPermit
        {
            get => _isUseWorkPermit;
            set => SetProperty(ref _isUseWorkPermit, value);
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
        private void CreateNewBaseInfoEmployee()
        {
            NewBaseInfoEmployee = new();

            NewBaseInfoEmployee.PropertyChanged += (s, e) => CreateEmployeeCommandAsync?.RaiseCanExecuteChanged();
        }

        //NewAssignmentContract
        private AssignmentContractDisplay _newAssignmentContract = null!;
        public AssignmentContractDisplay NewAssignmentContract
        {
            get => _newAssignmentContract;
            private set => SetProperty(ref _newAssignmentContract, value);
        }
        private void CreateNewAssignmentContract()
        {
            NewAssignmentContract = new
                (
                    new AssignmentResponse
                        (
                            Guid.Empty, 
                            Guid.Empty, 
                            Guid.Empty, 
                            Guid.Empty, 
                            Guid.Empty, 
                            DateTime.UtcNow, 
                            DateTime.UtcNow, 
                            Guid.Empty
                        ),
                    new ContractResponse
                        (
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            DateTime.UtcNow,
                            DateTime.UtcNow,
                            decimal.Zero,
                            string.Empty
                        ),
                    [], [], [], [], []
                );

            NewAssignmentContract.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(AssignmentContractDisplay.Department))
                    await GetAllPositionByIdDepartmentAsync();

                AddAssignmentContractCommand?.RaiseCanExecuteChanged();
            };
        }

        //NewPersonalDocument
        private PersonalDocumentDisplay _newPersonalDocument = null!;
        public PersonalDocumentDisplay NewPersonalDocument
        {
            get => _newPersonalDocument;
            private set => SetProperty(ref _newPersonalDocument, value);
        }
        private void CreateNewPersonalDocument()
        {
            NewPersonalDocument = new(new PersonalDocumentResponse(Guid.Empty, Guid.Empty, string.Empty, string.Empty), []);

            NewPersonalDocument.PropertyChanged += (s, e) => AddPersonalDocumentCommand?.RaiseCanExecuteChanged();
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
            NewEducationDocument = new(new EducationDocumentResponse(string.Empty, string.Empty, string.Empty, string.Empty, DateTime.UtcNow.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, "0"), [], []);

            NewEducationDocument.PropertyChanged += (s, e) => AddEducationDocumentCommand?.RaiseCanExecuteChanged();
        }

        //NewWorkPermit
        private WorkPermitDisplay _newWorkPermit = null!;
        public WorkPermitDisplay NewWorkPermit
        {
            get => _newWorkPermit;
            private set => SetProperty(ref _newWorkPermit, value);
        }
        private void CreateNewWorkPermit()
        {
            NewWorkPermit = new(new WorkPermitResponse(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, DateTime.UtcNow, DateTime.UtcNow, string.Empty, string.Empty), [], []);

            NewWorkPermit.PropertyChanged += (s, e) => AddWorkPermitCommand?.RaiseCanExecuteChanged();
        }

        #endregion

        #region Employees

        public RelayCommandAsync CreateEmployeeCommandAsync { get; private set; }
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
                        LocalPersonalDocuments.Select(x => new CreateEmployeePersonalDocumentRequest(Guid.Parse(x.DocumentType.Id), x.DocumentNumber, x.DocumentSeries)).ToList(),
                        createContactInformation,
                        LocalEducationDocuments.Select(x => new CreateEducationDocumentRequest(Guid.Parse(x.EducationLevel.Id), Guid.Parse(x.DocumentType.Id), x.DocumentNumber, x.IssuedDate, x.OrganizationName, x.QualificationAwardedName, x.SpecialtyName, x.ProgramName, x.TotalHours)).ToList(),
                        LocalWorkPermits.Select(x => new CreateWorkPermitRequest(x.WorkPermitName, x.DocumentSeries, x.WorkPermitNumber, x.ProtocolNumber, x.SpecialtyName, x.IssuingAuthority, x.IssueDate, x.ExpiryDate, Guid.Parse(x.PermitType.Id), Guid.Parse(x.AdmissionStatus.Id))).ToList(),
                        SelectedEmployeeSpecialties.Select(x => new CreateEmployeeSpecialtyRequest(Guid.Parse(x.Id))).ToList(),
                        LocalAssignmentsContracts.Select(x => new CreateAssignmentRequest(Guid.Parse(x.Position.Id), Guid.Parse(x.Department.Id), Guid.Parse(x.Manager?.Id!), x.HireDate, x.TerminationDate, Guid.Parse(x.Status.Id), new CreateAssignmentContractRequest(Guid.Parse(x.EmployeeType.Id), x.ContractNumber, x.StartDate, x.EndDate, x.Salary))).ToList()
                    )
                );

            result.Switch
                (
                    () => MessageBox.Show("Успешное добавление!"),
                    errors => MessageBox.Show(result.StringMessage)
                );

            CreateNewWorkPermit();
            CreateNewBaseInfoEmployee();
            CreateNewPersonalDocument();
            CreateNewEducationDocument();
            CreateNewAssignmentContract();
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

        #region AmissionStatus

        public ObservableCollection<AdmissionStatusResponse> AdmissionStatuses { get; private init; } = [];
        private async Task GetAllAdmissionStatusAsync() => AdmissionStatuses.Load(await _admissionStatusReadOnlyService.GetAllAsync());

        #endregion

        #region PermiteType

        public ObservableCollection<PermitTypeResponse> PermitTypes { get; private init; } = [];
        private async Task GetAllPermitTypeAsync() => PermitTypes.Load(await _permitTypeReadOnlyService.GetAllAsync());

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

        public ObservableCollection<EducationDocumentDisplay> LocalEducationDocuments { get; private init; } = [];

        public RelayCommand AddEducationDocumentCommand { get; private set; }
        private void Execute_AddEducationDocumentCommand()
        {
            LocalEducationDocuments.Add(NewEducationDocument);

            CreateNewEducationDocument();
        }
        private bool CanExecute_AddEducationDocumentCommand()
            => NewEducationDocument.EducationLevel != null && NewEducationDocument.DocumentType != null &&
            !string.IsNullOrWhiteSpace(NewEducationDocument.DocumentNumber) && 
            !string.IsNullOrWhiteSpace(NewEducationDocument.IssuedDate.ToString()) &&
            NewWorkPermit.IssueDate != DateTime.MinValue &&
            !string.IsNullOrWhiteSpace(NewEducationDocument.OrganizationName);

        public RelayCommand<EducationDocumentDisplay>? DeleteEducationDocumentCommand { get; private set; }
        private void Execute_DeleteEducationDocumentCommand(EducationDocumentDisplay display)
            => LocalEducationDocuments.Remove(display);

        #endregion

        #region WorkPermit

        private WorkPermitDisplay _selectedWorkPermit = null!;
        public WorkPermitDisplay SelectedWorkPermit
        {
            get => _selectedWorkPermit;
            set => SetProperty(ref _selectedWorkPermit, value);
        }

        public ObservableCollection<WorkPermitDisplay> LocalWorkPermits { get; private init; } = [];

        public RelayCommand AddWorkPermitCommand { get; private set; }
        private void Execute_AddWorkPermitCommand()
        {
            LocalWorkPermits.Add(NewWorkPermit);

            CreateNewWorkPermit();
        }
        private bool CanExecute_AddWorkPermitCommand()
            => NewWorkPermit.AdmissionStatus != null && NewWorkPermit.PermitType != null &&
            !string.IsNullOrWhiteSpace(NewWorkPermit.WorkPermitName) &&
            !string.IsNullOrWhiteSpace(NewWorkPermit.WorkPermitNumber) &&
            !string.IsNullOrWhiteSpace(NewWorkPermit.SpecialtyName) &&
            !string.IsNullOrWhiteSpace(NewWorkPermit.IssuingAuthority) &&
            //NewWorkPermit.IssueDate != DateTime.MinValue && 
            !string.IsNullOrWhiteSpace(NewWorkPermit.IssueDate.ToString()) &&
            //NewWorkPermit.ExpiryDate != DateTime.MinValue &&
            !string.IsNullOrWhiteSpace(NewWorkPermit.ExpiryDate.ToString());

        public RelayCommand<WorkPermitDisplay> DeleteWorkPermitCommand { get; private set; }
        private void Execute_DeleteWorkPermitCommand(WorkPermitDisplay display) 
            => LocalWorkPermits.Remove(display);

        #endregion

        #region EmployeeType

        public ObservableCollection<EmployeeTypeResponse> EmployeeTypes { get; private init; } = [];
        private async Task GetAllEmployeeTypeAsync() => EmployeeTypes.Load(await _employeeTypeReadOnlyService.GetAllAsync());

        #endregion

        #region Specialty

        private SpecialtyResponse _selectedSpecialty = null!;
        public SpecialtyResponse SelectedSpecialty
        {
            get => _selectedSpecialty;
            set
            {
                SetProperty(ref _selectedSpecialty, value);

                AddEmployeeSpecialtyCommand?.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<SpecialtyResponse> Specialties { get; private init; } = [];
        private async Task GetAllSpecialty() => Specialties.Load(await _specialtyReadOnlyService.GetAllAsync());

        #endregion

        #region EmployeeSpecialty

        public ObservableCollection<SpecialtyResponse> SelectedEmployeeSpecialties { get; private init; } = [];
        
        public RelayCommand AddEmployeeSpecialtyCommand { get; private set; }
        private void Execute_AddEmployeeSpecialtyCommand()
        {
            SelectedEmployeeSpecialties.Add(SelectedSpecialty);
            SelectedSpecialty = null!;
        }
        private bool CanExecute_AddEmployeeSpecialtyCommand() => SelectedSpecialty != null;

        public RelayCommand<SpecialtyResponse> DeleteEmployeeSpecialtyCommand { get; private set; }
        private void Execute_DeleteEmployeeSpecialtyCommand(SpecialtyResponse display) 
            => SelectedEmployeeSpecialties.Remove(display);

        #endregion

        #region Department

        public ObservableCollection<DepartmentResponse> Departments { get; private init; } = [];
        private async Task GetAllDepartmentAsync() => Departments.Load(await _departmentReadOnlyService.GetAllAsync());

        #endregion

        #region Position

        public ObservableCollection<PositionResponse> Positions { get; private init; } = [];
        private async Task GetAllPositionByIdDepartmentAsync() 
            => Positions.Load(await _positionReadOnlyApiServiceFactory.Create(NewAssignmentContract.Department.Id.ToString()).GetAllAsync(), cleaning: true);

        #endregion

        #region Status

        public ObservableCollection<StatusResponse> Statuses { get; private init; } = [];
        private async Task GetAllStatusAsync() => Statuses.Load(await _statusReadOnlyService.GetAllAsync());

        #endregion

        #region Manager

        public ObservableCollection<EmployeeResponse> Managers { get; private init; } = [];
        private async Task GetAllManagerAsync() => Managers.Load(await _employeeService.GetAllAsync());

        #endregion

        #region AssignmentContract

        private AssignmentContractDisplay _selectedAssignmentContract = null!;
        public AssignmentContractDisplay SelectedAssignmentContract
        {
            get => _selectedAssignmentContract;
            set => SetProperty(ref _selectedAssignmentContract, value);
        }

        public ObservableCollection<AssignmentContractDisplay> LocalAssignmentsContracts { get; private init; } = [];

        public RelayCommand AddAssignmentContractCommand { get; private set; }
        private void Execute_AddAssignmentContractCommand()
        {
            LocalAssignmentsContracts.Add(NewAssignmentContract);

            CreateNewAssignmentContract();
        }
        private bool CanExecute_AddAssignmentContractCommand()
            => NewAssignmentContract.Department != null && NewAssignmentContract.Position != null &&
               NewAssignmentContract.EmployeeType != null && NewAssignmentContract.Status != null &&
               !string.IsNullOrWhiteSpace(NewAssignmentContract.HireDate.ToString()) &&
               !string.IsNullOrWhiteSpace(NewAssignmentContract.ContractNumber) &&
               !string.IsNullOrWhiteSpace(NewAssignmentContract.StartDate.ToString()) &&
               !string.IsNullOrWhiteSpace(NewAssignmentContract.EndDate.ToString()) &&
               !string.IsNullOrWhiteSpace(NewAssignmentContract.Salary.ToString());

        public RelayCommand<AssignmentContractDisplay> DeleteAssignmentContractCommand { get; private set; }
        private void Execute_DeleteAssignmentContractCommand(AssignmentContractDisplay display)
            => LocalAssignmentsContracts.Remove(display);

        #endregion
    }
}
