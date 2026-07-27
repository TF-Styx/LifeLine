using LifeLine.Employee.Service.Client.Services.Employee;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.ReferenceData;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee;
using Shared.Contracts.Request.EmployeeService.Employee;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class PersonalInfoVM : BaseEmployeeViewModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IReferenceDataCacheService _cacheService;
        private readonly ManagementEmployeeStateService _stateService;

        public PersonalInfoVM
            (
                IEmployeeService employeeService, 
                IReferenceDataCacheService cacheService, 
                ManagementEmployeeStateService stateService
            )
        {
            _employeeService = employeeService;
            _cacheService = cacheService;
            _stateService = stateService;

            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            UpdateCommandAsync = new RelayCommandAsync(Execute_UpdateCommandAsync, CanExecute_UpdateCommandAsync);
        }

        public string? Surname
        {
            get => field;
            set
            {
                SetProperty(ref field, value);

                CreateCommandAsync?.RaiseCanExecuteChanged();
                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }
        public string? Name
        {
            get => field;
            set
            {
                SetProperty(ref field, value);

                CreateCommandAsync?.RaiseCanExecuteChanged();
                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }
        public string? Patronymic
        {
            get => field;
            set
            {
                SetProperty(ref field, value);

                CreateCommandAsync?.RaiseCanExecuteChanged();
                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        public GenderDisplay? Gender
        {
            get => field;
            set
            {
                SetProperty(ref field, value);

                CreateCommandAsync?.RaiseCanExecuteChanged();
                UpdateCommandAsync?.RaiseCanExecuteChanged();
            }
        }
        public ReadOnlyObservableCollection<GenderDisplay> Genders => _cacheService.Genders;

        public Action? EmployeeSaved;

        public RelayCommandAsync CreateCommandAsync { get; private set; }
        private async Task Execute_CreateCommandAsync()
        {
            var request = new CreateEmployeeRequest
            (
                Surname!,
                Name!,
                Patronymic,
                Gender!.GenderId
            );

            var result = await _employeeService.AddAsync<CreateEmployeeRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var employeeHrResponse = new EmployeeHrItemResponse(result.Value, Surname!, Name!, Patronymic, null, true, []);

            _stateService.UpdateEmployeeData(employeeHrResponse);
            EmployeeSaved?.Invoke();
        }
        private bool CanExecute_CreateCommandAsync()
            => Gender != null && 
               !string.IsNullOrWhiteSpace(Surname) &&
               !string.IsNullOrWhiteSpace(Name);

        public RelayCommandAsync UpdateCommandAsync { get; private set; }
        private async Task Execute_UpdateCommandAsync()
        {
            var request = new UpdateEmployeeRequest
            (
                Surname!,
                Name!,
                Patronymic,
                Gender!.GenderId
            );

            var result = await _employeeService.UpdateEmployeeAsync(_stateService.EmployeeHr!.Id, request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var employeeHrResponse = _stateService.EmployeeHr with
            {
                Surname = Surname!,
                Name = Name!,
                Patronymic = Patronymic,
            };

            _stateService.UpdateEmployeeData(employeeHrResponse);
            EmployeeSaved?.Invoke();
        }
        private bool CanExecute_UpdateCommandAsync()
            => Gender != null && 
               _stateService.EmployeeHr != null && 
               !string.IsNullOrWhiteSpace(_stateService.EmployeeHr.Id) &&
               !string.IsNullOrWhiteSpace(Surname) &&
               !string.IsNullOrWhiteSpace(Name);

        public void ClearProperty()
        {
            Surname = string.Empty;
            Name = string.Empty;
            Patronymic = string.Empty;
            Gender = null;
        }
    }
}
