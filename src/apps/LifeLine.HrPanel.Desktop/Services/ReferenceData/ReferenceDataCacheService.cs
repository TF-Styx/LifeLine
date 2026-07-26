using LifeLine.Directory.Service.Client.Services.AdmissionStatus;
using LifeLine.Directory.Service.Client.Services.Branch;
using LifeLine.Directory.Service.Client.Services.Department;
using LifeLine.Directory.Service.Client.Services.DocumentType;
using LifeLine.Directory.Service.Client.Services.EducationLevel;
using LifeLine.Directory.Service.Client.Services.Hospital;
using LifeLine.Directory.Service.Client.Services.PermitType;
using LifeLine.Directory.Service.Client.Services.Position.Factories;
using LifeLine.Directory.Service.Client.Services.Status;
using LifeLine.Employee.Service.Client.Services.Employee;
using LifeLine.Employee.Service.Client.Services.EmployeeType;
using LifeLine.Employee.Service.Client.Services.Gender;
using LifeLine.Employee.Service.Client.Services.Specialty;
using LifeLine.HrPanel.Desktop.Models;
using Shared.WPF.Extensions;
using System.Collections.ObjectModel;

namespace LifeLine.HrPanel.Desktop.Services.ReferenceData
{
    public sealed class ReferenceDataCacheService : IReferenceDataCacheService
    {
        private readonly IGenderReadOnlyService _genderService;
        private readonly IStatusReadOnlyService _statusService;
        private readonly IDocumentTypeReadOnlyService _documentTypeService;
        private readonly IPermitTypeReadOnlyService _permitTypeService;
        private readonly IAdmissionStatusReadOnlyService _admissionStatusService;
        private readonly IEducationLevelReadOnlyService _educationLevelService;
        private readonly IEmployeeTypeReadOnlyService _employeeTypeService;
        private readonly ISpecialtyReadOnlyService _specialtyService;
        private readonly IEmployeeService _employeeService;
        private readonly IHospitalReadOnlyService _hospitalService;
        private readonly IBranchReadOnlyService _branchService;
        private readonly IDepartmentReadOnlyService _departmentService;
        private readonly IPositionReadOnlyApiServiceFactory _positionService;

        private readonly ObservableCollection<GenderDisplay> _genders = [];
        private readonly ObservableCollection<StatusDisplay> _statuses = [];
        private readonly ObservableCollection<DocumentTypeDisplay> _documentTypes = [];
        private readonly ObservableCollection<PermitTypeDisplay> _permitTypes = [];
        private readonly ObservableCollection<AdmissionStatusDisplay> _admissionStatuses = [];
        private readonly ObservableCollection<EducationLevelDisplay> _educationLevels = [];
        private readonly ObservableCollection<EmployeeTypeDisplay> _employeeTypes = [];
        private readonly ObservableCollection<SpecialtyDisplay> _specialties = [];
        private readonly ObservableCollection<ManagerDisplay> _managers = [];
        private readonly ObservableCollection<HospitalDisplay> _hospitals = [];
        private readonly ObservableCollection<BranchDisplay> _branches = [];
        private readonly ObservableCollection<DepartmentDisplay> _departments = [];
        private readonly ObservableCollection<PositionDisplay> _positions = [];

        public ReferenceDataCacheService
            (
                IGenderReadOnlyService genderService,
                IStatusReadOnlyService statusService,
                IDocumentTypeReadOnlyService documentTypeService,
                IPermitTypeReadOnlyService permitTypeService,
                IAdmissionStatusReadOnlyService admissionStatusService,
                IEducationLevelReadOnlyService educationLevelService,
                IEmployeeTypeReadOnlyService employeeTypeService,
                ISpecialtyReadOnlyService specialtyService,
                IEmployeeService employeeService,
                IHospitalReadOnlyService hospitalService,
                IBranchReadOnlyService branchService,
                IDepartmentReadOnlyService departmentService,
                IPositionReadOnlyApiServiceFactory positionService
            )
        {
            _genderService = genderService;
            _statusService = statusService;
            _documentTypeService = documentTypeService;
            _permitTypeService = permitTypeService;
            _admissionStatusService = admissionStatusService;
            _educationLevelService = educationLevelService;
            _employeeTypeService = employeeTypeService;
            _specialtyService = specialtyService;
            _employeeService = employeeService;
            _hospitalService = hospitalService;
            _branchService = branchService;
            _departmentService = departmentService;
            _positionService = positionService;

            Genders = new(_genders);
            Statuses = new(_statuses);
            DocumentTypes = new(_documentTypes);
            PermitTypes = new(_permitTypes);
            AdmissionStatuses = new(_admissionStatuses);
            EducationLevels = new(_educationLevels);
            EmployeeTypes = new(_employeeTypes);
            Specialties = new(_specialties);
            Managers = new(_managers);
            Hospitals = new(_hospitals);
            Branches = new(_branches);
            Departments = new(_departments);
            Positions = new(_positions);
        }

        public async Task InitializeAsync()
        {
            var gendersTask = GetAllGenders();
            var statusesTask = GetAllStatuses();
            var documentTypesTask = GetAllDocumentTypes();
            var permitTypesTask = GetAllPermitTypes();
            var admissionStatusesTask = GetAllAdmissionStatuses();
            var educationLevelsTask = GetAllEducationLevels();
            var employeeTypesTask = GetAllEmployeeTypes();
            var specialtiesTask = GetAllSpecialties();
            var managersTask = GetAllManagers();
            var hospitalsTask = GetAllHospitals();
            var branchesTask = GetAllBranches();
            var departmentsTask = GetAllDepartments();
            var positionsTask = GetAllPositions();

            await Task.WhenAll
                (
                    gendersTask, 
                    statusesTask, 
                    documentTypesTask, 
                    permitTypesTask, 
                    admissionStatusesTask, 
                    educationLevelsTask, 
                    employeeTypesTask, 
                    specialtiesTask, 
                    managersTask, 
                    hospitalsTask,
                    branchesTask,
                    departmentsTask,
                    positionsTask
                );
        }

        public ReadOnlyObservableCollection<GenderDisplay> Genders { get; }
        public async Task GetAllGenders()
        {
            var genders = await _genderService.GetAllAsync();

            _genders.Load([.. genders.Select(gender => new GenderDisplay(gender))], cleaning: true);
        }

        public ReadOnlyObservableCollection<StatusDisplay> Statuses { get; }
        private async Task GetAllStatuses()
        {
            var statuses = await _statusService.GetAllAsync();

            _statuses.Load([.. statuses.Select(status => new StatusDisplay(status))], cleaning: true);
        }

        public ReadOnlyObservableCollection<DocumentTypeDisplay> DocumentTypes { get; }
        private async Task GetAllDocumentTypes()
        {
            var documentTypes = await _documentTypeService.GetAllAsync();

            _documentTypes.Load([.. documentTypes.Select(documentType => new DocumentTypeDisplay(documentType))], cleaning: true);
        }

        public ReadOnlyObservableCollection<PermitTypeDisplay> PermitTypes { get; }
        private async Task GetAllPermitTypes()
        {
            var permitTypes = await _permitTypeService.GetAllAsync();

            _permitTypes.Load([.. permitTypes.Select(permitType => new PermitTypeDisplay(permitType))], cleaning: true);
        }

        public ReadOnlyObservableCollection<AdmissionStatusDisplay> AdmissionStatuses { get; }
        private async Task GetAllAdmissionStatuses()
        {
            var admissionStatuses = await _admissionStatusService.GetAllAsync();

            _admissionStatuses.Load([.. admissionStatuses.Select(admissionStatuses => new AdmissionStatusDisplay(admissionStatuses))], cleaning: true);
        }

        public ReadOnlyObservableCollection<EducationLevelDisplay> EducationLevels { get; }
        private async Task GetAllEducationLevels()
        {
            var educationLevels = await _educationLevelService.GetAllAsync();

            _educationLevels.Load([.. educationLevels.Select(educationLevel => new EducationLevelDisplay(educationLevel))], cleaning: true);
        }

        public ReadOnlyObservableCollection<EmployeeTypeDisplay> EmployeeTypes { get; }
        private async Task GetAllEmployeeTypes()
        {
            var employeeTypes = await _employeeTypeService.GetAllAsync();

            _employeeTypes.Load([.. employeeTypes.Select(employeeType => new EmployeeTypeDisplay(employeeType))], cleaning: true);
        }

        public ReadOnlyObservableCollection<SpecialtyDisplay> Specialties { get; }
        private async Task GetAllSpecialties()
        {
            var specialties = await _specialtyService.GetAllAsync();

            _specialties.Load([.. specialties.Select(specialty => new SpecialtyDisplay(specialty))], cleaning: true);
        }

        public ReadOnlyObservableCollection<ManagerDisplay> Managers { get; }
        private async Task GetAllManagers()
        {
            var managers = await _employeeService.GetAllAsync();

            _managers.Load([.. managers.Select(manager => new ManagerDisplay(manager))], cleaning: true);
        }

        public ReadOnlyObservableCollection<HospitalDisplay> Hospitals { get; }
        private async Task GetAllHospitals()
        {
            var hospitals = await _hospitalService.GetAllAsync();

            _hospitals.Load([.. hospitals.Select(hospital => new HospitalDisplay(hospital))], cleaning: true);
        }

        public ReadOnlyObservableCollection<BranchDisplay> Branches { get; }
        private async Task GetAllBranches()
        {
            var branches = await _branchService.GetAllAsync();

            _branches.Load([.. branches.Select(branch => new BranchDisplay(branch))], cleaning: true);
        }

        public ReadOnlyObservableCollection<DepartmentDisplay> Departments { get; }
        private async Task GetAllDepartments()
        {
            var departments = await _departmentService.GetAllAsync();

            _departments.Load([.. departments.Select(department => new DepartmentDisplay(department))], cleaning: true);
        }

        public ReadOnlyObservableCollection<PositionDisplay> Positions { get; }
        private async Task GetAllPositions()
        {
            var positions = await _positionService.Create(Guid.NewGuid().ToString()).GetAllAsync();

            _positions.Load([.. positions.Select(position => new PositionDisplay(position))], cleaning: true);
        }
    }
}
