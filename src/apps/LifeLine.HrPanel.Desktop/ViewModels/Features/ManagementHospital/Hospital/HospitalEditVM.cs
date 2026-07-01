using LifeLine.Directory.Service.Client.Services.Hospital;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.DirectoryService.Hospital;
using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Abstract;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Hospital
{
    public sealed class HospitalEditVM : BaseViewModel
    {
        private readonly IHospitalService _service;
        private readonly ManagementHospitalStateService _stateService;

        public HospitalEditVM(IHospitalService service, ManagementHospitalStateService stateService)
        {
            _service = service;
            _stateService = stateService;

            NewHospitalDisplay();

            CreateHospitalCommandAsync = new RelayCommandAsync(Execute_CreateHospitalCommandAsync, CanExecute_CreateHospitalCommandAsync);
            UpdateHospitalCommandAsync = new RelayCommandAsync(Execute_UpdateHospitalCommandAsync, CanExecute_UpdateHospitalCommandAsync);

            CloseEditPanelCommand = new RelayCommand(Execute_CloseEditPanelCommand);
        }

        // Property
        private HospitalDisplay? _hospitalProp;
        public HospitalDisplay? HospitalProp
        {
            get => _hospitalProp;
            set
            {
                SetProperty(ref _hospitalProp, value);

                CreateHospitalCommandAsync?.RaiseCanExecuteChanged();
                UpdateHospitalCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void NewHospitalDisplay()
        {
            HospitalProp = new HospitalDisplay
            (
                new HospitalResponse
                (
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    new HospitalDataAddressResponse
                    (
                        string.Empty, 
                        string.Empty, 
                        string.Empty, 
                        string.Empty
                    )
                )
            );

            HospitalProp.PropertyChanged += (s, e) =>
            {
                CreateHospitalCommandAsync?.RaiseCanExecuteChanged();
                UpdateHospitalCommandAsync?.RaiseCanExecuteChanged();
            };
        }

        public void LoadHospital(HospitalDisplay display)
        {
            if (display == null)
            {
                NewHospitalDisplay();
                ClearHospitalForm();
                return;
            }

            HospitalProp!.Name = display.Name;
            HospitalProp!.Description = display.Description;
            HospitalProp!.Phone = display.Phone;
            HospitalProp!.Email = display.Email;
            HospitalProp!.PostalCode = display.PostalCode;
            HospitalProp!.Region = display.Region;
            HospitalProp!.City = display.City;
            HospitalProp!.Street = display.Street;
        }

        public void ClearHospitalForm()
        {
            HospitalProp!.Name = string.Empty;
            HospitalProp!.Description = string.Empty;
            HospitalProp!.Phone = string.Empty;
            HospitalProp!.Email = string.Empty;
            HospitalProp!.PostalCode = string.Empty;
            HospitalProp!.Region = string.Empty;
            HospitalProp!.City = string.Empty;
            HospitalProp!.Street = string.Empty;
        }

        public Action<HospitalDisplay>? HospitalSaved;

        public RelayCommandAsync CreateHospitalCommandAsync { get; private set; }
        private async Task Execute_CreateHospitalCommandAsync()
        {
            if (HospitalProp == null)
            {
                MessageBox.Show("Данные пусты!");
                return;
            }

            var request = new CreateHospitalRequest
            (
                HospitalProp.Name,
                HospitalProp.Description,
                HospitalProp.Phone,
                HospitalProp.Email,
                new CreateHospitalDataAddressRequest
                (
                    HospitalProp.PostalCode,
                    HospitalProp.Region,
                    HospitalProp.City,
                    HospitalProp.Street
                )
            );

            var result = await _service.AddAsync<CreateHospitalRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var newDisplay = new HospitalDisplay
            (
                new HospitalResponse
                (
                    result.Value,
                    HospitalProp.Name,
                    HospitalProp.Description,
                    HospitalProp.Phone,
                    HospitalProp.Email,
                    new HospitalDataAddressResponse
                    (
                        HospitalProp.PostalCode,
                        HospitalProp.Region,
                        HospitalProp.City,
                        HospitalProp.Street
                    )
                )
            );

            HospitalSaved?.Invoke(newDisplay);

            NewHospitalDisplay();
            ClearHospitalForm();
            OnCloseRequested?.Invoke();
        }
        private bool CanExecute_CreateHospitalCommandAsync()
            => HospitalProp != null &&
               !string.IsNullOrWhiteSpace(HospitalProp?.Name) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.Phone) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.Email) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.PostalCode) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.Region) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.City) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.Street);

        public RelayCommandAsync UpdateHospitalCommandAsync { get; private set; }
        private async Task Execute_UpdateHospitalCommandAsync()
        {
            if (HospitalProp == null)
            {
                MessageBox.Show("Данные пусты!");
                return;
            }

            var request = new UpdateHospitalRequest
            (
                HospitalProp.Name,
                HospitalProp.Description,
                HospitalProp.Phone,
                HospitalProp.Email,
                new UpdateHospitalDataAddressRequest
                (
                    HospitalProp.PostalCode,
                    HospitalProp.Region,
                    HospitalProp.City,
                    HospitalProp.Street
                )
            );

            if (_stateService.Hospital == null)
            {
                MessageBox.Show("Больница не выбрана!");
                return;
            }

            var result = await _service.UpdateAsync(_stateService.Hospital.Id, request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var newDisplay = new HospitalDisplay
            (
                new HospitalResponse
                (
                    _stateService.Hospital.Id,
                    request.Name,
                    request.Description,
                    request.Phone,
                    request.Email,
                    new HospitalDataAddressResponse
                    (
                        request.Address.PostalCode,
                        request.Address.Region,
                        request.Address.City,
                        request.Address.Street
                    )
                )
            );

            HospitalSaved?.Invoke(newDisplay);

            NewHospitalDisplay();
            ClearHospitalForm();
            OnCloseRequested?.Invoke();
        }
        private bool CanExecute_UpdateHospitalCommandAsync()
            => HospitalProp != null && 
               !string.IsNullOrWhiteSpace(_stateService.Hospital?.Id) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.Name) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.Phone) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.Email) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.PostalCode) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.Region) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.City) &&
               !string.IsNullOrWhiteSpace(HospitalProp?.Street);

        public Action? OnCloseRequested;
        public RelayCommand CloseEditPanelCommand { get; private set; }
        private void Execute_CloseEditPanelCommand()
        {
            ClearHospitalForm();
            OnCloseRequested?.Invoke();
        }
    }
}
