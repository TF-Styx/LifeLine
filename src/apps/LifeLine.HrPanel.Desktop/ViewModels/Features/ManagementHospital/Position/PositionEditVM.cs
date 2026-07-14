using LifeLine.Directory.Service.Client.Services.Position.Factories;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.DirectoryService.Position;
using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Abstract;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Position
{
    public sealed class PositionEditVM : BaseViewModel
    {
        private readonly IPositionApiServiceFactory _positionApiServiceFactory;
        private readonly ManagementHospitalStateService _stateService;

        public PositionEditVM(IPositionApiServiceFactory positionApiServiceFactory, ManagementHospitalStateService stateService)
        {
            _positionApiServiceFactory = positionApiServiceFactory;
            _stateService = stateService;

            NewPositionDisplay();

            CreatePositionCommandAsync = new RelayCommandAsync(Execute_CreatePositionCommandAsync, CanExecute_CreatePositionCommandAsync);
            UpdatePositionCommandAsync = new RelayCommandAsync(Execute_UpdatePositionCommandAsync, CanExecute_UpdatePositionCommandAsync);

            CloseEditPanelCommand = new RelayCommand(Execute_CloseEditPanelCommand);
        }

        private string? _editingId;

        // Property
        private PositionDisplay? _positionProp;
        public PositionDisplay? PositionProp
        {
            get => _positionProp;
            set
            {
                SetProperty(ref _positionProp, value);

                CreatePositionCommandAsync?.RaiseCanExecuteChanged();
                UpdatePositionCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void NewPositionDisplay()
        {
            PositionProp = new PositionDisplay(new PositionResponse(string.Empty, string.Empty, string.Empty));

            _editingId = null;

            PositionProp.PropertyChanged += (s, e) =>
            {
                CreatePositionCommandAsync?.RaiseCanExecuteChanged();
                UpdatePositionCommandAsync?.RaiseCanExecuteChanged();
            };
        }

        public void LoadPosition(PositionDisplay display)
        {
            if (display == null)
            {
                NewPositionDisplay();
                ClearPositionForm();
                return;
            }

            _editingId = display.PositionId;

            PositionProp!.Name = display.Name;
            PositionProp!.Description = display.Description;
        }

        public void ClearPositionForm()
        {
            PositionProp!.Name = string.Empty;
            PositionProp!.Description = string.Empty;

            _editingId = null;
        }

        public Action<PositionDisplay>? PositionSaved;

        public RelayCommandAsync CreatePositionCommandAsync { get; private set; }
        private async Task Execute_CreatePositionCommandAsync()
        {
            if (PositionProp == null || _stateService.Department == null)
            {
                MessageBox.Show("Данные пусты или отдел не выбран!");
                return;
            }

            var request = new CreatePositionRequest(PositionProp.Name, PositionProp.Description);

            var result = await _positionApiServiceFactory.Create(_stateService.Department.Id).AddAsync<CreatePositionRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var newDisplay = new PositionDisplay(new PositionResponse(result.Value, PositionProp.Name, PositionProp.Description));

            PositionSaved?.Invoke(newDisplay);

            NewPositionDisplay();
            ClearPositionForm();
            OnCloseRequested?.Invoke();
        }
        private bool CanExecute_CreatePositionCommandAsync()
            => PositionProp != null &&
               !string.IsNullOrWhiteSpace(_stateService.Hospital?.Id) &&
               !string.IsNullOrWhiteSpace(_stateService.Branch?.Id) &&
               !string.IsNullOrWhiteSpace(_stateService.Department?.Id) &&
               !string.IsNullOrWhiteSpace(PositionProp?.Name);

        public RelayCommandAsync UpdatePositionCommandAsync { get; private set; }
        private async Task Execute_UpdatePositionCommandAsync()
        {
            if (PositionProp == null || _stateService.Department == null || string.IsNullOrWhiteSpace(_editingId))
            {
                MessageBox.Show("Данные пусты, или не выбран отдел/должность!");
                return;
            }

            var request = new UpdatePositionRequest(PositionProp.Name, PositionProp.Description);

            var result = await _positionApiServiceFactory.Create(_stateService.Department.Id).UpdateAsync(_editingId, request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var newDisplay = new PositionDisplay(new PositionResponse(_editingId, PositionProp.Name, PositionProp.Description));

            PositionSaved?.Invoke(newDisplay);

            NewPositionDisplay();
            ClearPositionForm();
            OnCloseRequested?.Invoke();
        }
        private bool CanExecute_UpdatePositionCommandAsync()
            => PositionProp != null &&
               !string.IsNullOrWhiteSpace(_editingId) &&
               !string.IsNullOrWhiteSpace(_stateService.Branch?.Id) &&
               !string.IsNullOrWhiteSpace(_stateService.Department?.Id) &&
               !string.IsNullOrWhiteSpace(_stateService.Position?.Id) &&
               !string.IsNullOrWhiteSpace(PositionProp?.Name);

        public Action? OnCloseRequested;
        public RelayCommand CloseEditPanelCommand { get; private set; }
        private void Execute_CloseEditPanelCommand()
        {
            ClearPositionForm();
            OnCloseRequested?.Invoke();
        }
    }
}
