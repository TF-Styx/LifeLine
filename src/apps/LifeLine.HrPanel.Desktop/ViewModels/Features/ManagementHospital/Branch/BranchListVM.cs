using LifeLine.Directory.Service.Client.Services.Branch;
using LifeLine.HrPanel.Desktop.Models;
using Shared.WPF.Commands;
using Shared.WPF.Extensions;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Branch
{
    public sealed class BranchListVM : BaseViewModel
    {
        private readonly IBranchService _service;
        private readonly ManagementHospitalStateService _stateService;

        public BranchListVM(IBranchService service, ManagementHospitalStateService stateService)
        {
            _service = service;
            _stateService = stateService; 
            
            _stateService.HospitalContextChanged += async (hospitalId) =>
            {
                CurrentHospitalName = _stateService.Hospital?.Name;

                Branches.Clear();

                if (!string.IsNullOrWhiteSpace(hospitalId))
                    await GetBranchesByHospital(hospitalId);
            };

            EditBranchCommand = new RelayCommand<BranchDisplay?>(Execute_EditBranchCommand);
            DeleteBranchCommandAsync = new RelayCommandAsync<BranchDisplay>(Execute_DeleteBranchCommandAsync);
        }

        private string? _currentHospitalName;
        public string? CurrentHospitalName
        {
            get => _currentHospitalName;
            set => SetProperty(ref _currentHospitalName, value);
        }

        public ObservableCollection<BranchDisplay> Branches { get; private init; } = [];
        private async Task GetBranchesByHospital(string hospitalId)
        {
            var branchesResult = await _service.GetAllByIdHospitalAsync(hospitalId);

            if (branchesResult.IsFailure)
            {
                MessageBox.Show(branchesResult.StringMessage);
                return;
            }

            var branches = branchesResult.Value;

            Branches.Load([.. branches.Select(branch => new BranchDisplay(branch))]);
        }

        // Selected
        private BranchDisplay? _branch;
        public BranchDisplay? Branch
        {
            get => _branch;
            set
            {
                if (SetProperty(ref _branch, value) && value != null)
                    _stateService.SetSelectedBranch(value.GetUnderlineModel());
            }
        }

        public Action<BranchDisplay?>? RequestEditBranch;
        public RelayCommand<BranchDisplay?> EditBranchCommand { get; private set; }
        private void Execute_EditBranchCommand(BranchDisplay? display) => RequestEditBranch?.Invoke(display ?? null);

        public void UpdateBranchInList(BranchDisplay display)
        {
            if (display == null)
                return;

            var existing = Branches.FirstOrDefault(x => x.BranchId == display.BranchId);

            if (existing != null)
            {
                var index = Branches.IndexOf(existing);
                Branches[index] = display;
            }
            else
            {
                Branches.Add(display);
            }
        }

        public Action<BranchDisplay>? BranchDeleted;
        public RelayCommandAsync<BranchDisplay> DeleteBranchCommandAsync { get; private set; }
        private async Task Execute_DeleteBranchCommandAsync(BranchDisplay display)
        {
            if (display == null)
            {
                MessageBox.Show("Выберите филиал для удаления!");
                return;
            }

            var result = await _service.DeleteAsync(display.BranchId);

            if (result.IsFailure)
            {
                MessageBox.Show(result.StringMessage);
                return;
            }

            Branches.Remove(display);
            BranchDeleted?.Invoke(display);

            if (_stateService.Branch?.Id == display.BranchId)
                _stateService.ClearBranch();
        }
    }
}
