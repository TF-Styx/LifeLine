using LifeLine.Employee.Service.Client.Services.Employee;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.GenerateImage;
using LifeLine.HrPanel.Desktop.Services.ReferenceData;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.AssignmentContract;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.EducationDocument;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.PersonalDocument;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.WorkPermit;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.Employee
{
    internal sealed class EmployeeEditVM : BaseViewModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IGenerateImageService _generateImageService;
        private readonly ManagementEmployeeStateService _stateService;
        private readonly IReferenceDataCacheService _cacheService;

        public EmployeeEditVM
            (
                ManagementEmployeeStateService stateService,
                IEmployeeService employeeService,
                IGenerateImageService generateImageService,
                IReferenceDataCacheService cacheService,
                PersonalInfoVM personalInfo,
                PersonalPhotoVM personalPhoto,
                ContactInformationVM contactInformation,
                SpecialtiesVM specialties,
                PersonalDocumentManagementVM personalDocuments,
                EducationDocumentManagementVM educationDocuments,
                WorkPermitManagementVM workPermits,
                AssignmentContractManagementVM assignments
            )
        {
            _stateService = stateService;
            _employeeService = employeeService;
            _generateImageService = generateImageService;
            _cacheService = cacheService;

            _personalInfo = personalInfo;
            _personalPhoto = personalPhoto;
            _сontactInformation = contactInformation;
            _specialties = specialties;

            _personalDocuments = personalDocuments;
            _educationDocuments = educationDocuments;
            _workPermits = workPermits;
            _assignments = assignments;

            CloseEditPanelCommand = new RelayCommand(Execute_CloseEditPanelCommand);
        }

        #region FeaturesVM

        private PersonalInfoVM? _personalInfo;
        public PersonalInfoVM? PersonalInfo
        {
            get => _personalInfo;
            set => SetProperty(ref _personalInfo, value);
        }

        private PersonalPhotoVM? _personalPhoto;
        public PersonalPhotoVM? PersonalPhoto
        {
            get => _personalPhoto;
            set => SetProperty(ref _personalPhoto, value);
        }

        private ContactInformationVM? _сontactInformation;
        public ContactInformationVM? ContactInformation
        {
            get => _сontactInformation;
            set => SetProperty(ref _сontactInformation, value);
        }

        private SpecialtiesVM? _specialties;
        public SpecialtiesVM? Specialties
        {
            get => _specialties;
            set => SetProperty(ref _specialties, value);
        }

        #endregion

        #region ManagementVM

        private PersonalDocumentManagementVM _personalDocuments;
        public PersonalDocumentManagementVM PersonalDocuments
        {
            get => _personalDocuments;
            set => SetProperty(ref _personalDocuments, value);
        }

        private EducationDocumentManagementVM _educationDocuments;
        public EducationDocumentManagementVM EducationDocuments
        {
            get => _educationDocuments;
            set => SetProperty(ref _educationDocuments, value);
        }

        private WorkPermitManagementVM _workPermits;
        public WorkPermitManagementVM WorkPermits
        {
            get => _workPermits;
            set => SetProperty(ref _workPermits, value);
        }

        private AssignmentContractManagementVM _assignments;
        public AssignmentContractManagementVM Assignments
        {
            get => _assignments;
            set => SetProperty(ref _assignments, value);
        }

        #endregion

        public async Task LoadEmployeeAsync(EmployeeHrDisplay employee)
        {
            if (employee == null) 
                return;

            var currentAssignments = new List<AssignmentResponseInfo>();

            if (!string.IsNullOrEmpty(employee.BranchId))
            {
                currentAssignments.Add
                (
                    new AssignmentResponseInfo
                    (
                        employee.BranchId,
                        employee.DepartmentId!,
                        employee.PositionId!,
                        employee.StatusId!
                    )
                );
            }

            _stateService.SetSelectedEmployee
                (
                    new EmployeeHrItemResponse
                    (
                        employee.Id, 
                        employee.Surname, 
                        employee.Name, 
                        employee.Patronymic, 
                        employee.PersonalPhotoUrlDB, 
                        true, 
                        currentAssignments
                    )
                );

            var bio = await _employeeService.GetBioEmployeeAsync(employee.Id);

            if (bio == null) 
                return;

            UpdatePersonalInfo(bio);
            await UpdatePersonalPhoto(bio);
            UpdateContactInformation(bio.ContactInformation);
            UpdateSpecialties(bio.SpecialtyIds);
        }

        private void UpdatePersonalInfo(EmployeeBioResponse bio)
        {
            PersonalInfo!.EmployeeId = bio.EmployeeId.ToString();
            PersonalInfo!.Surname = bio.Surname;
            PersonalInfo!.Name = bio.Name;
            PersonalInfo!.Patronymic = bio.Patronymic;

            PersonalInfo!.Gender = _cacheService.Genders
                .FirstOrDefault(x => x.GenderId == bio.GenderId.ToString());
        }

        private async Task UpdatePersonalPhoto(EmployeeBioResponse bio)
        {
            PersonalPhoto!.EmployeeId = bio.EmployeeId.ToString();
            PersonalPhoto!.PhotoUrl = bio.PersonalPhotoKey;
            PersonalPhoto!.Photo = await _generateImageService.GenerateAsync(bio.PersonalPhotoKey);
        }

        private void UpdateContactInformation(ContactInformationResponse? contacts)
        {
            ContactInformation!.EmployeeId = _stateService.EmployeeHr!.Id;

            if (contacts == null)
            {
                ContactInformation.ClearProperty();
                return;
            }

            var d = ContactInformation.Display;

            d.ContactInformationId = contacts.Id;
            d.PersonalPhone = contacts.PersonalPhone;
            d.CorporatePhone = contacts.CorporatePhone;
            d.PersonalEmail = contacts.PersonalEmail;
            d.CorporateEmail = contacts.CorporateEmail;
            d.PostalCode = contacts.PostalCode;
            d.Region = contacts.Region;
            d.City = contacts.City;
            d.Street = contacts.Street;
            d.Building = contacts.Building;
            d.Apartment = contacts.Apartment;

            d.CommitChanges();
        }

        private void UpdateSpecialties(List<string>? specialtyIds)
        {
            Specialties!.EmployeeId = _stateService.EmployeeHr!.Id;
            Specialties!.LocalEmployeeSpecialties.Clear();

            if (specialtyIds == null || !specialtyIds.Any()) return;

            foreach (var id in specialtyIds)
            {
                var specialty = _cacheService.Specialties
                    .FirstOrDefault(x => x.SpecialtyId == id.ToString());

                if (specialty != null)
                    Specialties.LocalEmployeeSpecialties.Add(specialty);
            }
        }

        public void ClearForm()
        {
            PersonalInfo?.ClearProperty();
            PersonalPhoto?.ClearProperty();
            ContactInformation?.ClearProperty();
            Specialties?.ClearProperty();

            PersonalDocuments?.CloseEditPanel();
            EducationDocuments?.CloseEditPanel();
            WorkPermits?.CloseEditPanel();
            Assignments?.CloseEditPanel();

            _stateService.ClearEmployee();
        }

        public void ClearFormFields()
        {
            //PersonalInfo?.ClearProperty();
            PersonalPhoto?.ClearProperty();
            ContactInformation?.ClearProperty();
            Specialties?.ClearProperty();

            PersonalDocuments?.CloseEditPanel();
            EducationDocuments?.CloseEditPanel();
            WorkPermits?.CloseEditPanel();
            Assignments?.CloseEditPanel();
        }

        public Action? OnClosed;
        public RelayCommand CloseEditPanelCommand { get; private set; }
        private void Execute_CloseEditPanelCommand()
        {
            ClearForm();
            OnClosed?.Invoke();
        }
    }
}
