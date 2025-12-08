using LifeLine.Employee.Service.Client.Services.Employee;
using LifeLine.Employee.Service.Client.Services.Employee.ContactInformation;
using LifeLine.Employee.Service.Client.Services.Gender;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.EmployeeService.ContactInformation;
using Shared.Contracts.Request.EmployeeService.Employee;
using Shared.WPF.Commands;
using Shared.WPF.Enums;
using Shared.WPF.Extensions;
using Shared.WPF.Services.NavigationService.Pages;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Pages
{
    public sealed class EditContactInformationEmployeePageVM : BasePageViewModel, IUpdatable, IAsyncInitializable
    {
        private readonly INavigationPage _navigationPage;

        private readonly IEmployeeService _employeeService;
        private readonly IGenderService _genderService;
        private readonly IContactInformationApiServiceFactory _contactInformationApiServiceFactory;

        public EditContactInformationEmployeePageVM
            (
                INavigationPage navigationPage,

                IEmployeeService employeeService,
                IGenderService genderService,
                IContactInformationApiServiceFactory contactInformationApiServiceFactory
            )
        {
            _navigationPage = navigationPage;

            _employeeService = employeeService;
            _genderService = genderService;
            _contactInformationApiServiceFactory = contactInformationApiServiceFactory;

            UpdateContactInformationEmployeeCommand = new RelayCommandAsync(Execute_UpdateContactInformationEmployeeCommand);
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            await GetAllGenderAsync();

            SelectedGender = Genders.FirstOrDefault(x => x.GenderId == GenderDisplay.GenderId)!;

            IsInitialize = true;
        }

        public void Update<TData>(TData value, TransmittingParameter parameter)
        {
            if (value is ValueTuple<EmployeeDetailsDisplay, GenderDisplay, ContactInformationDisplay> tuple)
            {
                CurrentEmployeeDetails = tuple.Item1;
                GenderDisplay = tuple.Item2;
                ContactInformationDisplay = tuple.Item3;
            }
        }

        #region Dispalay

        private EmployeeDetailsDisplay _currentEmployeeDetails = null!;
        public EmployeeDetailsDisplay CurrentEmployeeDetails
        {
            get => _currentEmployeeDetails;
            set => SetProperty(ref _currentEmployeeDetails, value);
        }

        private GenderDisplay _genderDisplay = null!;
        public GenderDisplay GenderDisplay
        {
            get => _genderDisplay;
            set => SetProperty(ref _genderDisplay, value);
        }

        private GenderDisplay? _selectedGender;
        public GenderDisplay? SelectedGender
        {
            get => _selectedGender;
            set => SetProperty(ref _selectedGender, value);
        }

        public ObservableCollection<GenderDisplay> Genders { get; private init; } = [];
        private async Task GetAllGenderAsync()
        {
            var genders = await _genderService.GetAllAsync();

            Genders.Load(genders.Select(gender => new GenderDisplay(gender)).ToList());
        }

        private ContactInformationDisplay _contactInformationDisplay = null!;
        public ContactInformationDisplay ContactInformationDisplay
        {
            get => _contactInformationDisplay;
            set => SetProperty(ref _contactInformationDisplay, value);
        }

        #endregion

        public RelayCommandAsync UpdateContactInformationEmployeeCommand { get; private set; }
        private async Task Execute_UpdateContactInformationEmployeeCommand()
        {
            var resultUpdateEmployee = await _employeeService.UpdateEmployeeAsync
                (
                    CurrentEmployeeDetails.EmployeeId,
                    new UpdateEmployeeRequest
                        (
                            CurrentEmployeeDetails.Surname,
                            CurrentEmployeeDetails.Name,
                            CurrentEmployeeDetails.Patronymic,
                            SelectedGender.GenderId.ToString()
                        )
                );

            var resultUpdateContactInformation = await _contactInformationApiServiceFactory.Create(CurrentEmployeeDetails.EmployeeId).UpdateContactInformationAsync
                (
                    new UpdateContactInformationRequest
                        (
                            ContactInformationDisplay.ContactInformationId,
                            CurrentEmployeeDetails.EmployeeId,
                            ContactInformationDisplay.PersonalPhone,
                            ContactInformationDisplay.CorporatePhone,
                            ContactInformationDisplay.PersonalEmail,
                            ContactInformationDisplay.CorporateEmail,
                            ContactInformationDisplay.PostalCode,
                            ContactInformationDisplay.Region,
                            ContactInformationDisplay.City,
                            ContactInformationDisplay.Street,
                            ContactInformationDisplay.Building,
                            ContactInformationDisplay.Apartment
                        )
                );

            if (resultUpdateEmployee.IsSuccess && resultUpdateContactInformation.IsSuccess)
            {
                _navigationPage.TransmittingValue
                    (
                        (
                            CurrentEmployeeDetails,
                            GenderDisplay,
                            ContactInformationDisplay
                        ),
                        FrameName.MainFrame,
                        PageName.EmployeePage,
                        TransmittingParameter.Update
                    );
            }
            else
            {
                MessageBox.Show($"Обновление сотрудника: {resultUpdateEmployee.StringMessage}\n" +
                                $"Обновление контактной информации: {resultUpdateContactInformation.StringMessage}");
            }
        }
    }
}
