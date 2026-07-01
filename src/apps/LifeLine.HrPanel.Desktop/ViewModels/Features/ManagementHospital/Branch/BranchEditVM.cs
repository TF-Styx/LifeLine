using LifeLine.Directory.Service.Client.Services.Branch;
using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Request.DirectoryService.Branch;
using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Abstract;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Branch
{
    public sealed class BranchEditVM : BaseViewModel
    {
        private readonly IBranchService _service;
        private readonly ManagementHospitalStateService _stateService;

        public BranchEditVM(IBranchService service, ManagementHospitalStateService stateService)
        {
            _service = service;
            _stateService = stateService;

            NewBranchDisplay();

            CreateBranchCommandAsync = new RelayCommandAsync(Execute_CreateBranchCommandAsync, CanExecute_CreateBranchCommandAsync);
            UpdateBranchCommandAsync = new RelayCommandAsync(Execute_UpdateBranchCommandAsync, CanExecute_UpdateBranchCommandAsync);

            CloseEditPanelCommand = new RelayCommand(Execute_CloseEditPanelCommand);
        }

        // Property
        private BranchDisplay? _branchProp;
        public BranchDisplay? BranchProp
        {
            get => _branchProp;
            set
            {
                SetProperty(ref _branchProp, value);

                CreateBranchCommandAsync?.RaiseCanExecuteChanged();
                UpdateBranchCommandAsync?.RaiseCanExecuteChanged();
            }
        }

        private void NewBranchDisplay()
        {
            BranchProp = new BranchDisplay
            (
                new BranchResponse
                (
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    string.Empty, 
                    new BranchDataAddressResponse
                    (
                        string.Empty, 
                        string.Empty, 
                        string.Empty, 
                        string.Empty
                    )
                )
            );

            BranchProp.PropertyChanged += (s, e) =>
            {
                CreateBranchCommandAsync?.RaiseCanExecuteChanged();
                UpdateBranchCommandAsync?.RaiseCanExecuteChanged();
            };
        }

        public void LoadBranch(BranchDisplay display)
        {
            if (display == null)
            {
                NewBranchDisplay();
                ClearBranchForm();
                return;
            }

            BranchProp!.Name = display.Name;
            BranchProp!.Description = display.Description;
            BranchProp!.Phone = display.Phone;
            BranchProp!.Email = display.Email;
            BranchProp!.PostalCode = display.PostalCode;
            BranchProp!.Region = display.Region;
            BranchProp!.City = display.City;
            BranchProp!.Street = display.Street;
        }

        public void ClearBranchForm()
        {
            BranchProp!.Name = string.Empty;
            BranchProp!.Description = string.Empty;
            BranchProp!.Phone = string.Empty;
            BranchProp!.Email = string.Empty;
            BranchProp!.PostalCode = string.Empty;
            BranchProp!.Region = string.Empty;
            BranchProp!.City = string.Empty;
            BranchProp!.Street = string.Empty;
        }

        public Action<BranchDisplay>? BranchSaved;

        public RelayCommandAsync CreateBranchCommandAsync { get; private set; }
        private async Task Execute_CreateBranchCommandAsync()
        {
            if (BranchProp == null)
            {
                MessageBox.Show("Данные пусты!");
                return;
            }

            if (_stateService.Hospital == null)
            {
                MessageBox.Show("Больница не выбрана!");
                return;
            }

            var request = new CreateBranchRequest
            (
                BranchProp.Name,
                BranchProp.Description,
                BranchProp.Phone,
                BranchProp.Email,
                _stateService.Hospital.Id,
                new CreateBranchDataAddressRequest
                (
                    BranchProp.PostalCode,
                    BranchProp.Region,
                    BranchProp.City,
                    BranchProp.Street
                )
            );

            var result = await _service.AddAsync<CreateBranchRequest, string>(request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var newDisplay = new BranchDisplay
            (
                new BranchResponse
                (
                    result.Value,
                    BranchProp.Name,
                    BranchProp.Description,
                    BranchProp.Phone,
                    BranchProp.Email,
                    _stateService.Hospital.Id,
                    new BranchDataAddressResponse
                    (
                        BranchProp.PostalCode,
                        BranchProp.Region,
                        BranchProp.City,
                        BranchProp.Street
                    )
                )
            );

            BranchSaved?.Invoke(newDisplay);

            NewBranchDisplay();
            ClearBranchForm();
            OnCloseRequested?.Invoke();
        }
        private bool CanExecute_CreateBranchCommandAsync()
            => BranchProp != null &&
               !string.IsNullOrWhiteSpace(_stateService.Hospital?.Id) &&
               !string.IsNullOrWhiteSpace(BranchProp?.Name) &&
               !string.IsNullOrWhiteSpace(BranchProp?.Phone) &&
               !string.IsNullOrWhiteSpace(BranchProp?.Email) &&
               !string.IsNullOrWhiteSpace(BranchProp?.PostalCode) &&
               !string.IsNullOrWhiteSpace(BranchProp?.Region) &&
               !string.IsNullOrWhiteSpace(BranchProp?.City) &&
               !string.IsNullOrWhiteSpace(BranchProp?.Street);

        public RelayCommandAsync UpdateBranchCommandAsync  { get; private set; }
        private async Task Execute_UpdateBranchCommandAsync()
        {
            if (BranchProp == null)
            {
                MessageBox.Show("Данные пусты!");
                return;
            }

            if (_stateService.Hospital == null)
            {
                MessageBox.Show("Больница не выбрана!");
                return;
            }

            var request = new UpdateBranchRequest
            (
                BranchProp.Name,
                BranchProp.Description,
                BranchProp.Phone,
                BranchProp.Email,
                _stateService.Hospital.Id,
                new UpdateBranchDataAddressRequest
                (
                    BranchProp.PostalCode,
                    BranchProp.Region,
                    BranchProp.City,
                    BranchProp.Street
                )
            );

            if (_stateService.Branch == null)
            {
                MessageBox.Show("Филиал не выбран!");
                return;
            }

            var result = await _service.UpdateAsync(_stateService.Branch.Id, request);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            var newDisplay = new BranchDisplay
            (
                new BranchResponse
                (
                    _stateService.Branch.Id,
                    BranchProp.Name,
                    BranchProp.Description,
                    BranchProp.Phone,
                    BranchProp.Email,
                    _stateService.Hospital.Id,
                    new BranchDataAddressResponse
                    (
                        BranchProp.PostalCode,
                        BranchProp.Region,
                        BranchProp.City,
                        BranchProp.Street
                    )
                )
            );

            BranchSaved?.Invoke(newDisplay);

            NewBranchDisplay();
            ClearBranchForm();
            OnCloseRequested?.Invoke();
        }
        private bool CanExecute_UpdateBranchCommandAsync()
            => BranchProp != null &&
               !string.IsNullOrWhiteSpace(_stateService.Hospital?.Id) &&
               !string.IsNullOrWhiteSpace(_stateService.Branch?.Id) &&
               !string.IsNullOrWhiteSpace(BranchProp?.Name) &&
               !string.IsNullOrWhiteSpace(BranchProp?.Phone) &&
               !string.IsNullOrWhiteSpace(BranchProp?.Email) &&
               !string.IsNullOrWhiteSpace(BranchProp?.PostalCode) &&
               !string.IsNullOrWhiteSpace(BranchProp?.Region) &&
               !string.IsNullOrWhiteSpace(BranchProp?.City) &&
               !string.IsNullOrWhiteSpace(BranchProp?.Street);

        public Action? OnCloseRequested;
        public RelayCommand CloseEditPanelCommand { get; private set; }
        private void Execute_CloseEditPanelCommand()
        {
            ClearBranchForm();
            OnCloseRequested?.Invoke();
        }
    }
}
