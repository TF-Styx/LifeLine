using LifeLine.Directory.Service.Client.Services.Department;
using LifeLine.HrPanel.Desktop.Models;
using Shared.WPF.Commands;
using Shared.WPF.Extensions;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Department
{
    public sealed class DepartmentListVM : BaseViewModel
    {
        private readonly IDepartmentService _service;
        private readonly ManagementHospitalStateService _stateService;

        public DepartmentListVM(IDepartmentService service, ManagementHospitalStateService stateService)
        {
            _service = service;
            _stateService = stateService;

            _stateService.BranchContextChanged += async (branchId) =>
            {
                CurrentBranchName = _stateService.Branch?.Name;

                Departments.Clear();

                if (!string.IsNullOrWhiteSpace(branchId))
                    await GetDepartmentsByBranchId(branchId);
            };

            EditDepartmentCommand = new RelayCommand<DepartmentDisplay?>(Execute_EditDepartmentCommand);
            DeleteDepartmentCommandAsync = new RelayCommandAsync<DepartmentDisplay>(Execute_DeleteDepartmentCommandAsync);
        }

        private string? _currentBranchName;
        public string? CurrentBranchName
        {
            get => _currentBranchName;
            set => SetProperty(ref _currentBranchName, value);
        }

        public ObservableCollection<DepartmentDisplay> Departments { get; private init; } = [];
        private async Task GetDepartmentsByBranchId(string branchId)
        {
            var departmentsResult = await _service.GetAllByBranchIdAsync(branchId);

            if (departmentsResult.IsFailure)
            {
                MessageBox.Show(departmentsResult.StringMessage);
                return;
            }

            var departments = departmentsResult.Value;

            Departments.Load([.. departments.Select(department => new DepartmentDisplay(department))]);
        }

        // Selected
        private DepartmentDisplay? _department;
        public DepartmentDisplay? Department
        {
            get => _department;
            set
            {
                if (SetProperty(ref _department, value) && value != null)
                    _stateService.SetSelectedDepartment(value.GetUnderlineModel());
            }
        }

        public Action<DepartmentDisplay?>? RequestEditDepartment;
        public RelayCommand<DepartmentDisplay?> EditDepartmentCommand { get; private set; }
        private void Execute_EditDepartmentCommand(DepartmentDisplay? display) => RequestEditDepartment?.Invoke(display ?? null);

        public void UpdateDepartmentInList(DepartmentDisplay display)
        {
            if (display == null)
                return;

            var existing = Departments.FirstOrDefault(x => x.DepartmentId == display.DepartmentId);

            if (existing != null)
            {
                var index = Departments.IndexOf(existing);
                Departments[index] = display;
            }
            else
            {
                Departments.Add(display);
            }
        }

        public Action<DepartmentDisplay>? DepartmentDeleted;
        public RelayCommandAsync<DepartmentDisplay> DeleteDepartmentCommandAsync { get; private set; }
        private async Task Execute_DeleteDepartmentCommandAsync(DepartmentDisplay display)
        {
            if (display == null)
            {
                MessageBox.Show("Выберите отдел для удаления!");
                return;
            }

            var result = await _service.DeleteAsync(display.DepartmentId);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            Departments.Remove(display);
            DepartmentDeleted?.Invoke(display);

            if (_stateService.Department?.Id == display.DepartmentId)
                _stateService.ClearDepartment();
        }
    }
}
