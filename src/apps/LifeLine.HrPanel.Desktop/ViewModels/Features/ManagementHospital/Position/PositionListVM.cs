using LifeLine.Directory.Service.Client.Services.Position.Factories;
using LifeLine.HrPanel.Desktop.Models;
using Shared.WPF.Commands;
using Shared.WPF.Extensions;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Position
{
    public sealed class PositionListVM : BaseViewModel
    {
        private readonly IPositionApiServiceFactory _positionApiServiceFactory;
        private readonly ManagementHospitalStateService _stateService;

        public PositionListVM(IPositionApiServiceFactory positionApiServiceFactory, ManagementHospitalStateService stateService)
        {
            _positionApiServiceFactory = positionApiServiceFactory;
            _stateService = stateService;

            _stateService.DepartmentContextChanged += async (departmentId) =>
            {
                CurrentDepartmentName = _stateService.Department?.Name;

                Positions.Clear();

                if (!string.IsNullOrWhiteSpace(departmentId))
                    await GetAllPositionsByDepartmentId(departmentId);
            };

            EditPositionCommand = new RelayCommand<PositionDisplay?>(Execute_EditPositionCommand);
            DeletePositionCommandAsync = new RelayCommandAsync<PositionDisplay>(Execute_DeletePositionCommandAsync);
        }

        private string? _currentDepartmentName;
        public string? CurrentDepartmentName
        {
            get => _currentDepartmentName;
            set => SetProperty(ref _currentDepartmentName, value);
        }

        public ObservableCollection<PositionDisplay> Positions { get; private init; } = [];
        private async Task GetAllPositionsByDepartmentId(string departmentId)
        {
            var positions = await _positionApiServiceFactory.Create(departmentId).GetAllAsync();

            Positions.Load([.. positions.Select(position => new PositionDisplay(position))]);
        }

        // Selected
        private PositionDisplay? _position;
        public PositionDisplay? Position
        {
            get => _position;
            set
            {
                if (SetProperty(ref _position, value) && value != null)
                    _stateService.SetSelectedPosition(value.GetUnderlineModel());
            }
        }

        public Action<PositionDisplay?>? RequestEditPosition { get; set; }
        public RelayCommand<PositionDisplay?> EditPositionCommand { get; private set; }
        private void Execute_EditPositionCommand(PositionDisplay? display) => RequestEditPosition?.Invoke(display ?? null);

        public void UpdatePositionInList(PositionDisplay display)
        {
            if (display == null)
                return;

            var existing = Positions.FirstOrDefault(x => x.PositionId == display.PositionId);

            if (existing != null)
            {
                var index = Positions.IndexOf(existing);
                Positions[index] = display;
            }
            else
            {
                Positions.Add(display);
            }
        }

        public Action<PositionDisplay>? PositionDeleted;
        public RelayCommandAsync<PositionDisplay> DeletePositionCommandAsync { get; private set; }
        private async Task Execute_DeletePositionCommandAsync(PositionDisplay display)
        {
            if (display == null && _stateService.Department == null)
            {
                MessageBox.Show("Выберите отдел для удаления!");
                return;
            }

            var result = await _positionApiServiceFactory.Create(_stateService.Department!.Id).DeleteAsync(display!.PositionId);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            Positions.Remove(display);
            PositionDeleted?.Invoke(display);

            if (_stateService.Position?.Id == display.PositionId)
                _stateService.ClearPosition();
        }
    }
}
