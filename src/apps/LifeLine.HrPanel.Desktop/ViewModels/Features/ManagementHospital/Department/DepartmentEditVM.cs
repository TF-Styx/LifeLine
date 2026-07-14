using LifeLine.Directory.Service.Client.Services.Department;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.DirectoryService.Department;
using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Abstract;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Department
{
    public sealed class DepartmentEditVM : BaseViewModel
    {
        private readonly IDepartmentService _service;
        private readonly ManagementHospitalStateService _stateService;

        public DepartmentEditVM(IDepartmentService service, ManagementHospitalStateService stateService)
        {
            _service = service;
            _stateService = stateService;

            NewDepartmentDisplay();

            CreateDepartmentCommandAsync = new RelayCommandAsync(Execute_CreateDepartmentCommandAsync, CanExecute_CreateDepartmentCommandAsync);
            UpdateDepartmentCommandAsync = new RelayCommandAsync(Execute_UpdateDepartmentCommandAsync, CanExecute_UpdateDepartmentCommandAsync);

            CloseEditPanelCommand = new RelayCommand(Execute_CloseEditPanelCommand);
        }

        private string? _editingId;

        // Property
        private DepartmentDisplay? _departmentProp;
        public DepartmentDisplay? DepartmentProp
        {
            get => _departmentProp;
            set
            {
                SetProperty(ref _departmentProp, value);

                CreateDepartmentCommandAsync?.RaiseCanExecuteChanged();
                UpdateDepartmentCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void NewDepartmentDisplay()
        {
            DepartmentProp = new DepartmentDisplay
            (
                new DepartmentResponse
                (
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty
                )
            );

            _editingId = null;

            DepartmentProp.PropertyChanged += (s, e) =>
            {
                CreateDepartmentCommandAsync?.RaiseCanExecuteChanged();
                UpdateDepartmentCommandAsync?.RaiseCanExecuteChanged();
            };
        }

        public void LoadDepartment(DepartmentDisplay display)
        {
            if (display == null)
            {
                NewDepartmentDisplay();
                ClearDepartmentForm();
                return;
            }

            _editingId = display.DepartmentId;

            DepartmentProp!.Name = display.Name;
            DepartmentProp!.Description = display.Description;
            DepartmentProp!.Building = display.Building;
        }

        public void ClearDepartmentForm()
        {
            DepartmentProp!.Name = string.Empty;
            DepartmentProp!.Description = string.Empty;
            DepartmentProp!.Building = string.Empty;

            _editingId = null;
        }

        public Action<DepartmentDisplay>? DepartmentSaved;

        public RelayCommandAsync CreateDepartmentCommandAsync { get; private set; }
        private async Task Execute_CreateDepartmentCommandAsync()
        {
            if (DepartmentProp == null || _stateService.Branch == null)
            {
                MessageBox.Show("Данные пусты или филиал не выбран!");
                return;
            }

            var request = new CreateDepartmentRequest
            (
                DepartmentProp.Name,
                DepartmentProp.Description,
                DepartmentProp.Building,
                _stateService.Branch.Id
            );

            var result = await _service.AddAsync<CreateDepartmentRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var newDisplay = new DepartmentDisplay
            (
                new DepartmentResponse
                (
                    result.Value,
                    DepartmentProp.Name,
                    DepartmentProp.Description,
                    DepartmentProp.Building,
                    _stateService.Branch.Id
                )
            );

            DepartmentSaved?.Invoke(newDisplay);

            NewDepartmentDisplay();
            ClearDepartmentForm();
            OnCloseRequested?.Invoke();
        }
        private bool CanExecute_CreateDepartmentCommandAsync()
            => DepartmentProp != null &&
               !string.IsNullOrWhiteSpace(_stateService.Hospital?.Id) &&
               !string.IsNullOrWhiteSpace(_stateService.Branch?.Id) &&
               !string.IsNullOrWhiteSpace(DepartmentProp?.Name) &&
               !string.IsNullOrWhiteSpace(DepartmentProp?.Building);

        public RelayCommandAsync UpdateDepartmentCommandAsync { get; private set; }
        private async Task Execute_UpdateDepartmentCommandAsync()
        {
            if (DepartmentProp == null || _stateService.Branch == null || string.IsNullOrWhiteSpace(_editingId))
            {
                MessageBox.Show("Данные пусты, или не выбран филиал/отдел!");
                return;
            }

            var request = new UpdateDepartmentRequest
            (
                DepartmentProp.Name,
                DepartmentProp.Description,
                DepartmentProp.Building,
                _stateService.Branch.Id
            );

            var result = await _service.UpdateAsync(_editingId, request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var newDisplay = new DepartmentDisplay
            (
                new DepartmentResponse
                (
                    _editingId,
                    DepartmentProp.Name,
                    DepartmentProp.Description,
                    DepartmentProp.Building,
                    _stateService.Branch.Id
                )
            );

            DepartmentSaved?.Invoke(newDisplay);

            NewDepartmentDisplay();
            ClearDepartmentForm();
            OnCloseRequested?.Invoke();
        }
        private bool CanExecute_UpdateDepartmentCommandAsync()
            => DepartmentProp != null &&
               !string.IsNullOrWhiteSpace(_editingId) &&
               !string.IsNullOrWhiteSpace(_stateService.Branch?.Id) &&
               !string.IsNullOrWhiteSpace(_stateService.Department?.Id) &&
               !string.IsNullOrWhiteSpace(DepartmentProp?.Name) &&
               !string.IsNullOrWhiteSpace(DepartmentProp?.Building);

        public Action? OnCloseRequested;
        public RelayCommand CloseEditPanelCommand { get; private set; }
        private void Execute_CloseEditPanelCommand()
        {
            ClearDepartmentForm();
            OnCloseRequested?.Invoke();
        }
    }
}
