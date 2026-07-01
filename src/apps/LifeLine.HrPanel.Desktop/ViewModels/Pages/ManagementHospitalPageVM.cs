using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Branch;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Department;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Hospital;
using LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital.Position;
using LifeLine.HrPanel.Desktop.ViewModels.Interfaces;
using Shared.WPF.Commands;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.ViewModels.Pages
{
    public sealed class ManagementHospitalPageVM : BaseViewModel
    {
        private readonly HospitalManagementVM _hospital;
        private readonly BranchManagementVM _branch;
        private readonly DepartmentManagementVM _department;
        private readonly PositionManagementVM _position;

        public ManagementHospitalPageVM
            (
                Func<HospitalManagementVM> hospitalFactory,
                Func<BranchManagementVM> branchFactory,
                Func<DepartmentManagementVM> departmentFactory,
                Func<PositionManagementVM> positionFactory
            )
        {
            _hospital= hospitalFactory();
            _branch = branchFactory();
            _department = departmentFactory();
            _position = positionFactory();

            ShowHospitalCommand = new RelayCommand(Execute_ShowHospitalCommand);
            ShowBranchCommand = new RelayCommand(Execute_ShowBranchCommand);
            ShowDepartmentCommand = new RelayCommand(Execute_ShowDepartmentCommand);
            ShowPositionCommand = new RelayCommand(Execute_ShowPositionCommand);

            Execute_ShowHospitalCommand();
        }

        private IChildren? _currentViewModel;
        public IChildren? CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public RelayCommand? ShowHospitalCommand { get; private set; }
        private void Execute_ShowHospitalCommand() => CurrentViewModel = _hospital;

        public RelayCommand? ShowBranchCommand { get; private set; }
        private void Execute_ShowBranchCommand() => CurrentViewModel = _branch;

        public RelayCommand? ShowDepartmentCommand { get; private set; }
        private void Execute_ShowDepartmentCommand() => CurrentViewModel = _department;

        public RelayCommand? ShowPositionCommand { get; private set; }
        private void Execute_ShowPositionCommand() => CurrentViewModel = _position;

    }
}
