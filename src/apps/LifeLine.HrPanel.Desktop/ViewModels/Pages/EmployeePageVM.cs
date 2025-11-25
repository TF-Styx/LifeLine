using LifeLine.Directory.Service.Client.Services.AdmissionStatus;
using LifeLine.Directory.Service.Client.Services.Department;
using LifeLine.Directory.Service.Client.Services.DocumentType;
using LifeLine.Directory.Service.Client.Services.EducationLevel;
using LifeLine.Directory.Service.Client.Services.PermitType;
using LifeLine.Directory.Service.Client.Services.Position.Factories;
using LifeLine.Directory.Service.Client.Services.Status;
using LifeLine.Employee.Service.Client.Services.Employee;
using LifeLine.Employee.Service.Client.Services.EmployeeType;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Response.DirectoryService;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Extensions;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Pages
{
    public sealed class EmployeePageVM : BasePageViewModel, IAsyncInitializable
    {
        private readonly IEmployeeService _employeeService;
        private readonly IStatusReadOnlyService _statusReadOnlyService;
        private readonly IPermitTypeReadOnlyService _permitTypeReadOnlyService;
        private readonly IDepartmentReadOnlyService _departmentReadOnlyService;
        private readonly IDocumentTypeReadOnlyService _documentTypeReadOnlyService;
        private readonly IEmployeeTypeReadOnlyService _employeeTypeReadOnlyService;
        private readonly IEducationLevelReadOnlyService _educationLevelReadOnlyService;
        private readonly IAdmissionStatusReadOnlyService _admissionStatusReadOnlyService;
        private readonly IPositionReadOnlyApiServiceFactory _positionReadOnlyApiServiceFactory;

        public EmployeePageVM
            (
                IEmployeeService employeeService, 
                IStatusReadOnlyService statusReadOnlyService,
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
            _statusReadOnlyService = statusReadOnlyService;
            _permitTypeReadOnlyService = permitTypeReadOnlyService;
            _departmentReadOnlyService = departmentReadOnlyService;
            _documentTypeReadOnlyService = documentTypeReadOnlyService;
            _employeeTypeReadOnlyService = employeeTypeReadOnlyService;
            _educationLevelReadOnlyService = educationLevelReadOnlyService;
            _admissionStatusReadOnlyService = admissionStatusReadOnlyService;
            _positionReadOnlyApiServiceFactory = positionReadOnlyApiServiceFactory;
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            await GetAllAdmissionStatus();
            await GetAllDepartmentAsync();
            await GetAllEducationLevel();
            await GetAllPositionAsync();
            await GetAllDocumentType();
            await GetAllEmployeeType();
            await GetAllStatusAsync();
            await GetAllPermiteType();
            await GetAllManager();
            await GetAllForHr();

            CreateNewBaseInfoEmployee();

            //string a = string.Empty;

            //foreach (var item in Managers)
            //    a += $"{item.Name}\n";

            //MessageBox.Show(a);

            //string b = string.Empty;

            //foreach (var item in EmployeeTypes)
            //    b += $"{item.Name}\n";

            //MessageBox.Show(b);
        }

        #region Display

        private EmployeeDetailsDisplay _currentEmployeeDetails = null!;
        public EmployeeDetailsDisplay CurrentEmployeeDetails
        {
            get => _currentEmployeeDetails;
            set => SetProperty(ref _currentEmployeeDetails, value);
        }

        //NewBaseInfoEmployee
        private EmployeeHrDisplay _newEmployeeHr = null!;
        public EmployeeHrDisplay NewEmployeeHr
        {
            get => _newEmployeeHr;
            private set => SetProperty(ref _newEmployeeHr, value);
        }
        private void CreateNewBaseInfoEmployee()
        {
            NewEmployeeHr = new(new EmployeeHrItemResponse(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, []), Departments, Positions, Statuses);

            //NewEmployeeHr.PropertyChanged += async (s, e) =>
            //{
            //    if (e.PropertyName == nameof(EmployeeHrDisplay.Department))
            //        await GetAllPositionByIdDepartmentAsync();
            //};
        }

        private GenderDisplay _genderDisplay = null!;
        public GenderDisplay GenderDisplay
        {
            get => _genderDisplay;
            set => SetProperty(ref _genderDisplay, value);
        }

        private AssignmentContractDisplay _assignmentContractDisplay = null!;
        public AssignmentContractDisplay AssignmentContractDisplay
        {
            get => _assignmentContractDisplay;
            set => SetProperty(ref _assignmentContractDisplay, value);
        }

        private ContactInformationDisplay _contactInformationDisplay = null!;
        public ContactInformationDisplay ContactInformationDisplay
        {
            get => _contactInformationDisplay;
            set => SetProperty(ref _contactInformationDisplay, value);
        }

        private EducationDocumentDisplay _educationDocumentDisplay = null!;
        public EducationDocumentDisplay EducationDocumentDisplay
        {
            get => _educationDocumentDisplay;
            set => SetProperty(ref _educationDocumentDisplay, value);
        }

        private PersonalDocumentDisplay _personalDocumentDisplay = null!;
        public PersonalDocumentDisplay PersonalDocumentDisplay
        {
            get => _personalDocumentDisplay;
            set => SetProperty(ref _personalDocumentDisplay, value);
        }

        private SpecialtyDisplay _specialtyDisplay = null!;
        public SpecialtyDisplay SpecialtyDisplay
        {
            get => _specialtyDisplay;
            set => SetProperty(ref _specialtyDisplay, value);
        }

        private WorkPermitDisplay _workPermitDisplay = null!;
        public WorkPermitDisplay WorkPermitDisplay
        {
            get => _workPermitDisplay;
            set => SetProperty(ref _workPermitDisplay, value);
        }

        #endregion

        private EmployeeHrDisplay _selectedEmployee = null!;
        public EmployeeHrDisplay SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                SetProperty(ref _selectedEmployee, value);

                ListClear();

                _ = GetEmployeeDetailsAsync(value.Id);
            }
        }

        private async Task GetEmployeeDetailsAsync(string id)
        {
            var details = await _employeeService.GetDetailsAsync(id);

            if (details == null)
            {
                MessageBox.Show("Не удалось получить детали пользователя!");
                return;
            }

            CurrentEmployeeDetails = new(details);

            GenderDisplay = new(new GenderResponse(details.Gender.GenderId.ToString(), details.Gender.GenderName));

            ContactInformationDisplay = new
                (
                    new ContactInformationResponse
                        (
                            details.ContactInformation!.ContactInformationId!, 
                            details.ContactInformation.PersonalPhone!, 
                            details.ContactInformation.CorporatePhone, 
                            details.ContactInformation.PersonalEmail!, 
                            details.ContactInformation.CorporateEmail!, 
                            details.ContactInformation.PostalCode!, 
                            details.ContactInformation.Region!, 
                            details.ContactInformation.City!, 
                            details.ContactInformation.Street!, 
                            details.ContactInformation.Building!, 
                            details.ContactInformation.Apartment
                        )
                );

            PersonalDocuments.Load
                (
                    details.PersonalDocuments?.Select
                        (
                            x => new PersonalDocumentDisplay
                                (
                                    new PersonalDocumentResponse
                                        (
                                            x.PersonalDocumentId,
                                            x.PersonalDocumentTypeId,
                                            x.PersonalDocumentNumber,
                                            x.PersonalDocumentSeries
                                        ), DocumentTypes
                                )
                        ).ToList()
                );


            EducationDocuments.Load
                (
                    details.EducationDocuments?.Select
                        (
                            x => new EducationDocumentDisplay
                                (
                                    new EducationDocumentResponse
                                        (
                                            details.EmployeeId.ToString(),
                                            x.EducationLevelId.ToString(),
                                            x.EducationDocumentTypeId.ToString(),
                                            x.EducationDocumentNumber,
                                            x.EducationIssuedDate.ToString(),
                                            x.OrganizationName,
                                            x.QualificationAwardedName,
                                            x.EducationSpecialtyName,
                                            x.ProgramName,
                                            x.TotalHours.ToString()
                                        ), EducationLevels, DocumentTypes
                                )
                        ).ToList()
                );

            Specialties.Load
                (
                    details.Specialties?.Select
                        (
                            x => new SpecialtyDisplay
                                (
                                    new SpecialtyResponse
                                        (
                                            x.SpecialtyId.ToString(), 
                                            x.SpecialtyName, 
                                            x.SpecialtyDescription
                                        )
                                )
                        ).ToList()
                );

            WorkPermits.Load
                (
                    details.WorkPermits?.Select
                        (
                            x => new WorkPermitDisplay
                                (
                                    new WorkPermitResponse
                                        (
                                            details.EmployeeId.ToString(),
                                            x.WorkPermitName,
                                            x.WorkPermitDocumentSeries,
                                            x.WorkPermitNumber,
                                            x.ProtocolNumber,
                                            x.WorkPermitSpecialtyName,
                                            x.IssuingAuthority,
                                            x.WorkPermitIssueDate,
                                            x.WorkPermitExpiryDate,
                                            x.PermitTypeId.ToString(),
                                            x.AdmissionStatusId.ToString()
                                        ),
                                    PermitTypes, AdmissionStatuses
                                )
                        ).ToList()
                );

            foreach (var item in details.Assignments!)
            {
                var contractsResponse = details.Contracts?.FirstOrDefault(x => x.ContractId == item.ContractId);

                var display = new AssignmentContractDisplay
                    (
                        new AssignmentResponse
                            (
                                item.AssignmentId,
                                details.EmployeeId,
                                item.PositionId,
                                item.DepartmentId,
                                item.ManagerId,
                                item.HireDate,
                                item.TerminationDate,
                                item.StatusId
                            ),
                        new ContractResponse
                            (
                                details.EmployeeId.ToString(),
                                contractsResponse!.ContractId.ToString(),
                                contractsResponse.ContractNumber,
                                contractsResponse.EmployeeTypeId.ToString(),
                                contractsResponse.ContractStartDate,
                                contractsResponse.ContractEndDate,
                                contractsResponse.Salary,
                                contractsResponse?.ContractFileKey
                            ),
                        Departments, Positions, Managers, Statuses, EmployeeTypes
                    );

                //display.SetDepartment(item.DepartmentId.ToString());
                //display.SetPosition(item.PositionId.ToString());
                //display.SetStatus(item.StatusId.ToString());
                //display.SetManager(item.ManagerId.ToString()!);
                //display.SetEmployeeType(contractsResponse!.EmployeeTypeId.ToString());

                //MessageBox.Show($"EmployeeType: {contractsResponse.EmployeeTypeId}");

                AssignmentContracts.Add(display);
            }

            #region Пока не надо

            //AssignmentContracts.Load
            //    (
            //        details.Assignments?.Join
            //            (
            //                details.Contracts ?? Enumerable.Empty<ContractDetailsResponseData>(),
            //                assignment => assignment.ContractId,
            //                contract => contract.ContractId,
            //                (assignment, contract) => new AssignmentContractDisplay
            //                    (
            //                        new AssignmentResponse
            //                            (
            //                                assignment.AssignmentId,
            //                                details.EmployeeId,
            //                                assignment.PositionId,
            //                                assignment.DepartmentId,
            //                                assignment.ManagerId,
            //                                assignment.HireDate,
            //                                assignment.TerminationDate,
            //                                assignment.StatusId
            //                            ),
            //                        new ContractResponse
            //                            (
            //                                details.EmployeeId.ToString(),
            //                                contract.ContractId.ToString(),
            //                                contract.ContractNumber,
            //                                contract.EmployeeType,
            //                                contract.ContractStartDate,
            //                                contract.ContractEndDate,
            //                                contract.Salary,
            //                                contract.ContractFileKey
            //                            ),
            //                        Departments, Positions, Managers, Statuses, EmployeeTypes
            //                    )
            //            ).ToList()
            //    );

            #endregion
        }

        public ObservableCollection<EmployeeHrDisplay> EmployeeHrs { get; private init; } = [];
        private async Task GetAllForHr()
        {
            var response = await _employeeService.GetAllForHrAsync();

            foreach (var item in response)
            {
                var display = new EmployeeHrDisplay(item, Departments, Positions, Statuses);

                if (item.Assignments.Count > 0)
                {
                    display.SetDepartment(item.Assignments.FirstOrDefault()!.DepartmentId);
                    display.SetPosition(item.Assignments.FirstOrDefault()!.PositionId);
                    display.SetStatus(item.Assignments.FirstOrDefault()!.StatusId);
                }

                EmployeeHrs.Add(display);
            }
        }

        public ObservableCollection<DepartmentDisplay> Departments { get; private init; } = [];
        private async Task GetAllDepartmentAsync()
        {
            var departments = await _departmentReadOnlyService.GetAllAsync();

            Departments.Load([.. departments.Select(department => new DepartmentDisplay(department))]);
        }

        public ObservableCollection<PositionDisplay> Positions { get; private init; } = [];
        private async Task GetAllPositionAsync()
        {
            var positions = await _positionReadOnlyApiServiceFactory.Create(Guid.NewGuid().ToString()).GetAllPosition();

            Positions.Load([.. positions.Select(position => new PositionDisplay(position))]);
        }

        public ObservableCollection<StatusDisplay> Statuses { get; private init; } = [];
        private async Task GetAllStatusAsync()
        {
            var statuses = await _statusReadOnlyService.GetAllAsync();

            Statuses.Load([.. statuses.Select(status => new StatusDisplay(status))]);
        }

        public ObservableCollection<ManagerDisplay> Managers { get; private init; } = [];
        private async Task GetAllManager()
        {
            var managers = await _employeeService.GetAllAsync();

            Managers.Load([.. managers.Select(manager => new ManagerDisplay(manager))]);
        }

        public ObservableCollection<EmployeeTypeDisplay> EmployeeTypes { get; private init; } = [];
        private async Task GetAllEmployeeType()
        {
            var employeeTypes = await _employeeTypeReadOnlyService.GetAllAsync();

            EmployeeTypes.Load([.. employeeTypes.Select(employeeType => new EmployeeTypeDisplay(employeeType))]);
        }

        public ObservableCollection<EducationLevelDisplay> EducationLevels { get; private init; } = [];
        private async Task GetAllEducationLevel()
        {
            var educationLevels = await _educationLevelReadOnlyService.GetAllAsync();

            EducationLevels.Load([.. educationLevels.Select(educationLevel => new EducationLevelDisplay(educationLevel))]);
        }

        public ObservableCollection<DocumentTypeDisplay> DocumentTypes { get; private init; } = [];
        private async Task GetAllDocumentType()
        {
            var documentTypes = await _documentTypeReadOnlyService.GetAllAsync();

            DocumentTypes.Load([.. documentTypes.Select(documentType => new DocumentTypeDisplay(documentType))]);
        }

        public ObservableCollection<PermitTypeDisplay> PermitTypes { get; private init; } = [];
        private async Task GetAllPermiteType()
        {
            var permitTypes = await _permitTypeReadOnlyService.GetAllAsync();

            PermitTypes.Load([.. permitTypes.Select(permiteType => new PermitTypeDisplay(permiteType))]);
        }

        public ObservableCollection<AdmissionStatusDisplay> AdmissionStatuses { get; private init; } = [];
        private async Task GetAllAdmissionStatus()
        {
            var admissionStatuses = await _admissionStatusReadOnlyService.GetAllAsync();

            AdmissionStatuses.Load([.. admissionStatuses.Select(admissionStatus => new AdmissionStatusDisplay(admissionStatus))]);
        }

        public ObservableCollection<PersonalDocumentDisplay> PersonalDocuments { get; private init; } = [];
        public ObservableCollection<EducationDocumentDisplay> EducationDocuments { get; private init; } = [];
        public ObservableCollection<SpecialtyDisplay> Specialties { get; private init; } = [];
        public ObservableCollection<WorkPermitDisplay> WorkPermits { get; private init; } = [];
        public ObservableCollection<AssignmentContractDisplay> AssignmentContracts { get; private init; } = [];

        private void ListClear()
        {
            PersonalDocuments.Clear();
            EducationDocuments.Clear();
            Specialties.Clear();
            WorkPermits.Clear();
            AssignmentContracts.Clear();
        }
    }
}
