using LifeLine.Employee.Service.Client.Services.Employee.ContactInformation;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee;
using Shared.Contracts.Request.EmployeeService.ContactInformation;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal class ContactInformationVM : BaseEmployeeViewModel
    {
        private readonly IContactInformationApiServiceFactory _contactInformationApiServiceFactory;
        private readonly ManagementEmployeeStateService _stateService;

        public ContactInformationVM
            (
                IContactInformationApiServiceFactory contactInformationApiServiceFactory,
                ManagementEmployeeStateService stateService
            )
        {
            _contactInformationApiServiceFactory = contactInformationApiServiceFactory;
            _stateService = stateService;

            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute);
            UpdateCommandAsync = new RelayCommandAsync(Execute_UpdateCommandAsync, CanExecute);

            CreateNewContactInformation();
        }

        private ContactInformationDisplay _display = null!;
        public ContactInformationDisplay Display
        {
            get => _display;
            private set
            {
                SetProperty(ref _display, value);

                CreateCommandAsync?.RaiseCanExecuteChanged();
                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }
        private void CreateNewContactInformation()
        {
            Display = new ContactInformationDisplay
            (
                new ContactInformationResponse
                (
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty
                )
            );

            Display.PropertyChanged += (s, e) =>
            {
                CreateCommandAsync?.RaiseCanExecuteChanged();
                UpdateCommandAsync?.RaiseCanExecuteChanged();
            };
        }

        public RelayCommandAsync CreateCommandAsync { get; private set; }
        private async Task Execute_CreateCommandAsync()
        {
            if (_stateService.EmployeeHr == null)
            {
                MessageBox.Show("Сотрудник не выбран!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var request = new CreateContactInformationRequest
            (
                Display.PersonalPhone,
                Display.CorporatePhone,
                Display.PersonalEmail,
                Display.CorporateEmail,
                Display.PostalCode,
                Display.Region,
                Display.City,
                Display.Street,
                Display.Building,
                Display.Apartment
            );

            var result = await _contactInformationApiServiceFactory.Create(_stateService.EmployeeHr!.Id)
                .AddAsync<CreateContactInformationRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            Display = new ContactInformationDisplay
            (
                new ContactInformationResponse
                (
                    result.Value,
                    Display.PersonalPhone,
                    Display.CorporatePhone,
                    Display.PersonalEmail,
                    Display.CorporateEmail,
                    Display.PostalCode,
                    Display.Region,
                    Display.City,
                    Display.Street,
                    Display.Building,
                    Display.Apartment
                )
            );
        }

        public RelayCommandAsync UpdateCommandAsync { get; private set; }
        private async Task Execute_UpdateCommandAsync()
        {
            if (_stateService.EmployeeHr == null)
            {
                MessageBox.Show("Сотрудник не выбран!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var request = new UpdateContactInformationRequest
            (
                Display.ContactInformationId,
                Display.PersonalPhone,
                Display.CorporatePhone,
                Display.PersonalEmail,
                Display.CorporateEmail,
                Display.PostalCode,
                Display.Region,
                Display.City,
                Display.Street,
                Display.Building,
                Display.Apartment
            );

            var result = await _contactInformationApiServiceFactory.Create(_stateService.EmployeeHr!.Id)
                .UpdateContactInformationAsync(request);

            Display.CommitChanges();
        }

        private bool CanExecute()
            => Display != null &&
               _stateService.EmployeeHr != null &&
               !string.IsNullOrWhiteSpace(_stateService.EmployeeHr.Id) &&
               !string.IsNullOrWhiteSpace(Display.PersonalPhone) &&
               !string.IsNullOrWhiteSpace(Display.PersonalEmail) &&
               !string.IsNullOrWhiteSpace(Display.PostalCode) &&
               !string.IsNullOrWhiteSpace(Display.Region) &&
               !string.IsNullOrWhiteSpace(Display.City) &&
               !string.IsNullOrWhiteSpace(Display.Street) &&
               !string.IsNullOrWhiteSpace(Display.Building);

        public void ClearProperty()
        {
            Display.PersonalPhone = string.Empty;
            Display.PersonalEmail = string.Empty;
            Display.CorporatePhone = string.Empty;
            Display.CorporateEmail = string.Empty;

            Display.PostalCode = string.Empty;
            Display.Region = string.Empty;
            Display.City = string.Empty;
            Display.Street = string.Empty;
            Display.Building = string.Empty;
            Display.Apartment = string.Empty;
        }
    }
}
