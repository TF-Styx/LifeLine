using LifeLine.Directory.Service.Client.Services.Hospital;
using LifeLine.HrPanel.Desktop.Models;
using Shared.WPF.Commands;
using Shared.WPF.Extensions;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Hospital
{
    public sealed class HospitalListVM : BaseViewModel, IAsyncInitializable
    {
        private readonly IHospitalService _service;
        private readonly ManagementHospitalStateService _stateService;

        public HospitalListVM(IHospitalService service, ManagementHospitalStateService stateService)
        {
            _service = service;
            _stateService = stateService;

            _stateService.HospitalContextChanged += async (hospitalId) => CurrentHospitalName = _stateService.Hospital?.Name;

            RefreshCommandAsync = new RelayCommandAsync(Execute_RefreshCommandAsync);
            EditHospitalCommand = new RelayCommand<HospitalDisplay?>(Execute_EditHospitalCommand);
            DeleteHospitalCommandAsync = new RelayCommandAsync<HospitalDisplay>(Execute_DeleteHospitalCommandAsync);
        }

        async Task IAsyncInitializable.InitializeAsync()
        {
            if (IsInitialize)
                return;

            IsInitialize = false;

            await GetAllHospitalsAsync();

            IsInitialize = true;
        }

        private string? _currentHospitalName;
        public string? CurrentHospitalName
        {
            get => _currentHospitalName;
            set => SetProperty(ref _currentHospitalName, value);
        }

        public ObservableCollection<HospitalDisplay> Hospitals { get; private init; } = [];
        private async Task GetAllHospitalsAsync()
        {
            var hospitals = await _service.GetAllAsync();

            Hospitals.Load([.. hospitals.Select(hospital => new HospitalDisplay(hospital))], cleaning: true);
        }

        public RelayCommandAsync RefreshCommandAsync { get; private set; }
        private async Task Execute_RefreshCommandAsync() => await GetAllHospitalsAsync();

        // Selected
        private HospitalDisplay? _hospital;
        public HospitalDisplay? Hospital
        {
            get => _hospital;
            set
            {
                if (SetProperty(ref _hospital, value) && value != null)
                    _stateService.SetSelectedHospital(value.GetUnderlineModel());
            }
        }

        public Action<HospitalDisplay?>? RequestEditHospital;
        public RelayCommand<HospitalDisplay?> EditHospitalCommand { get; private set; }
        private void Execute_EditHospitalCommand(HospitalDisplay? display) => RequestEditHospital?.Invoke(display ?? null);

        public void UpdateHospitalInList(HospitalDisplay display)
        {
            if (display == null)
                return;

            var existing = Hospitals.FirstOrDefault(x => x.HospitalId == display.HospitalId);

            if (existing != null)
            {
                var index = Hospitals.IndexOf(existing);
                Hospitals[index] = display;
            }
            else 
            { 
                Hospitals.Add(display); 
            }
        }

        public Action<HospitalDisplay>? HospitalDeleted;
        public RelayCommandAsync<HospitalDisplay> DeleteHospitalCommandAsync { get; private set; }
        private async Task Execute_DeleteHospitalCommandAsync(HospitalDisplay display)
        {
            if (display == null)
            {
                MessageBox.Show("Выберите больницу для удаления!");
                return;
            }

            var result = await _service.DeleteAsync(display.HospitalId);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            Hospitals.Remove(display);
            HospitalDeleted?.Invoke(display);

            if (_stateService.Hospital?.Id == display.HospitalId)
                _stateService.ClearHospital();
        }
    }
}
