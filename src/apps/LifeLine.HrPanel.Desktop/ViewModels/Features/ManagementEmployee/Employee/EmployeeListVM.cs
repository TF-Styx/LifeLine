using LifeLine.Employee.Service.Client.Services.Employee;
using LifeLine.HrPanel.Desktop.Models;
using LifeLine.HrPanel.Desktop.Services.GenerateImage;
using LifeLine.HrPanel.Desktop.Services.ReferenceData;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.Commands;
using Shared.WPF.Extensions;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee.Employee
{
    public sealed class EmployeeListVM : BaseViewModel, IAsyncInitializable
    {
        private readonly ManagementEmployeeStateService _stateService;
        private readonly IEmployeeService _employeeService;
        private readonly IReferenceDataCacheService _cacheService;
        private readonly IGenerateImageService _generateImageService;

        public EmployeeListVM
            (
                ManagementEmployeeStateService stateService,
                IEmployeeService employeeService,
                IReferenceDataCacheService cacheService,
                IGenerateImageService generateImageService
            )
        {
            _stateService = stateService;
            _employeeService = employeeService;
            _cacheService = cacheService;
            _generateImageService = generateImageService;

            EditCommand = new RelayCommand<EmployeeHrDisplay?>(Execute_EditCommand);
            DeleteCommandAsync = new RelayCommandAsync<EmployeeHrDisplay>(Execute_DeleteCommandAsync);
        }

        public async Task InitializeAsync()
        {
            await LoadAsync();
        }

        private EmployeeHrDisplay? _employee;
        public EmployeeHrDisplay? Employee
        {
            get => _employee;
            set
            {
                if (SetProperty(ref _employee, value) && value != null)
                    _stateService.SetSelectedEmployee(value.GetUnderlineModel());
            }
        }
        public ObservableCollection<EmployeeHrDisplay> Employees { get; private init; } = [];
        private async Task LoadAsync()
        {
            var result = await _employeeService.GetAllForHrAsync();

            Employees.Load([.. result.Select
            (
                employee => new EmployeeHrDisplay
                (
                    employee,
                    _cacheService.Branches,
                    _cacheService.Departments,
                    _cacheService.Positions,
                    _cacheService.Statuses
                )
            )], cleaning: true);

            for (int i = 0; i < Employees.Count; i++)
            {
                var item = Employees[i];
                var assignment = item.GetUnderlineModel().Assignments.FirstOrDefault();

                if (assignment != null)
                {
                    item.SetBranch(assignment.BranchId);
                    item.SetDepartment(assignment.DepartmentId);
                    item.SetPosition(assignment.PositionId);
                    item.SetStatus(assignment.StatusId);
                }
            }

            var photoTasks = Employees.Select(async item => item.PersonalPhoto = await _generateImageService.GenerateAsync(item.PersonalPhotoUrlDB)).ToList();

            await Task.WhenAll(photoTasks);
        }

        public Func<EmployeeHrDisplay?, Task>? RequestEdit;
        public RelayCommand<EmployeeHrDisplay?> EditCommand { get; private set; }
        private void Execute_EditCommand(EmployeeHrDisplay? display) => RequestEdit?.Invoke(display ?? null);

        public Action<EmployeeHrDisplay>? ItemDeleted;
        public RelayCommandAsync<EmployeeHrDisplay> DeleteCommandAsync { get; private set; }
        private async Task Execute_DeleteCommandAsync(EmployeeHrDisplay display)
        {
            var result = await _employeeService.SoftDeleteAsync(display.Id);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            Employees.Remove(display);
            ItemDeleted?.Invoke(display);

            if (_stateService.EmployeeHr?.Id == display.Id)
                _stateService.ClearEmployee();
        }

        public async Task UpdateEmployees(EmployeeHrItemResponse response)
        {
            var display = new EmployeeHrDisplay
            (
                response,
                _cacheService.Branches,
                _cacheService.Departments,
                _cacheService.Positions,
                _cacheService.Statuses
            ); 
            
            if (response.PersonalPhoto != null)
                display.PersonalPhoto = await _generateImageService.GenerateAsync(response.PersonalPhoto);

            var existing = Employees.FirstOrDefault(x => x.Id == response.Id);

            if (existing != null)
            {
                var index = Employees.IndexOf(existing);
                Employees[index] = display;
            }
            else
            {
                Employees.Add(display);
            }
        }

        public async Task RefreshEmployeePhotoInListAsync()
        {
            if (_stateService.EmployeeHr == null)
                return;

            var employeeInList = Employees.FirstOrDefault(x => x.Id == _stateService.EmployeeHr.Id);

            if (employeeInList == null)
                return;

            if (string.IsNullOrWhiteSpace(_stateService.EmployeeHr.PersonalPhoto))
            {
                employeeInList.PersonalPhoto = null;
                employeeInList.PersonalPhotoUrlDB = string.Empty;
                return;
            }

            try
            {
                string photoUrl = _stateService.EmployeeHr.PersonalPhoto;

                var newImage = await _generateImageService.GenerateAsync(photoUrl);

                employeeInList.PersonalPhoto = newImage;
                employeeInList.PersonalPhotoUrlDB = photoUrl;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[RefreshEmployeePhotoInListAsync] ОШИБКА генерации: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
