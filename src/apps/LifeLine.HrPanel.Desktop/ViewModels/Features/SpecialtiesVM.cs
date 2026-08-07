using LifeLine.Employee.Service.Client.Services.Employee.EmployeeSpecialtry;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.ReferenceData;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee;
using Shared.Contracts.Request.EmployeeService.EmployeeSpecialty;
using Shared.WPF.Commands;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class SpecialtiesVM : BaseEmployeeViewModel
    {
        private readonly IEmployeeSpecialtyApiServiceFactory _employeeSpecialtyApiServiceFactory;
        private readonly IReferenceDataCacheService _cacheService;
        private readonly ManagementEmployeeStateService _stateService;

        public SpecialtiesVM
            (
                IEmployeeSpecialtyApiServiceFactory employeeSpecialtyApiServiceFactory, 
                IReferenceDataCacheService cacheService,
                ManagementEmployeeStateService stateService
            )
        {
            _employeeSpecialtyApiServiceFactory = employeeSpecialtyApiServiceFactory;
            _cacheService = cacheService;
            _stateService = stateService;

            CreateCommandAsync = new RelayCommandAsync(Execute_CreateCommandAsync, CanExecute_CreateCommandAsync);
            DeleteCommandAsync = new RelayCommandAsync<SpecialtyDisplay>(Execute_DeleteCommandAsync);
        }

        public ObservableCollection<SpecialtyDisplay> LocalEmployeeSpecialties { get; private init; } = [];

        private SpecialtyDisplay _selectedSpecialty = null!;
        public SpecialtyDisplay SelectedSpecialty
        {
            get => _selectedSpecialty;
            set
            {
                SetProperty(ref _selectedSpecialty, value);

                CreateCommandAsync?.RaiseCanExecuteChanged();
            }
        }
        public ReadOnlyObservableCollection<SpecialtyDisplay> Specialties => _cacheService.Specialties;

        public RelayCommandAsync CreateCommandAsync { get; private set; }
        private async Task Execute_CreateCommandAsync()
        {
            if (SelectedSpecialty == null || _stateService.EmployeeHr == null || string.IsNullOrWhiteSpace(_stateService.EmployeeHr.Id))
            {
                MessageBox.Show("Не был выбран сотрудник!");
                return;
            }

            var request = new CreateEmployeeSpecialtyRequest(SelectedSpecialty.SpecialtyId);

            var result = await _employeeSpecialtyApiServiceFactory.Create(_stateService.EmployeeHr.Id)
                .CreateAsync(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            LocalEmployeeSpecialties.Add(SelectedSpecialty);
            ClearProperty();
        }
        private bool CanExecute_CreateCommandAsync()
            => SelectedSpecialty != null &&
               _stateService.EmployeeHr != null &&
               !string.IsNullOrWhiteSpace(_stateService.EmployeeHr.Id);

        public RelayCommandAsync<SpecialtyDisplay> DeleteCommandAsync { get; private set; }
        private async Task Execute_DeleteCommandAsync(SpecialtyDisplay display)
        {
            if (_stateService.EmployeeHr == null || string.IsNullOrWhiteSpace(_stateService.EmployeeHr!.Id))
            {
                MessageBox.Show("Не был выбран сотрудник!");
                return;
            }

            var result = await _employeeSpecialtyApiServiceFactory.Create(_stateService.EmployeeHr.Id)
                .DeleteEmployeeSpecialtyAsync(display.SpecialtyId);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            LocalEmployeeSpecialties.Remove(display);
        }

        public void ClearProperty()
            => SelectedSpecialty = null!;
    }
}
