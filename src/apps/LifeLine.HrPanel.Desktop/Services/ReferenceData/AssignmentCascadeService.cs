using LifeLine.Directory.Service.Client.Services.Branch;
using LifeLine.Directory.Service.Client.Services.Department;
using LifeLine.Directory.Service.Client.Services.Hospital;
using LifeLine.Directory.Service.Client.Services.Position.Factories;
using LifeLine.HrPanel.Desktop.Models;
using Shared.WPF.Extensions;
using System.Collections.ObjectModel;

namespace LifeLine.HrPanel.Desktop.Services.ReferenceData
{
    public sealed class AssignmentCascadeService : IAssignmentCascadeService
    {
        private readonly IHospitalReadOnlyService _hospitalService;
        private readonly IBranchReadOnlyService _branchService;
        private readonly IDepartmentReadOnlyService _departmentService;
        private readonly IPositionReadOnlyApiServiceFactory _positionFactory;

        private readonly ObservableCollection<HospitalDisplay> _hospitals = [];
        private readonly ObservableCollection<BranchDisplay> _branches = [];
        private readonly ObservableCollection<DepartmentDisplay> _departments = [];
        private readonly ObservableCollection<PositionDisplay> _positions = [];

        public AssignmentCascadeService
            (
                IHospitalReadOnlyService hospitalService,
                IBranchReadOnlyService branchService,
                IDepartmentReadOnlyService departmentService,
                IPositionReadOnlyApiServiceFactory positionFactory
            )
        {
            _hospitalService = hospitalService;
            _branchService = branchService;
            _departmentService = departmentService;
            _positionFactory = positionFactory;

            Hospitals = new(_hospitals);
            Branches = new(_branches);
            Departments = new(_departments);
            Positions = new(_positions);
        }

        public async Task InitializeAsync()
        {
            var hospitals = await _hospitalService.GetAllAsync();

            _hospitals.Load([.. hospitals.Select(hospital => new HospitalDisplay(hospital))], cleaning: true);
        }

        public ReadOnlyObservableCollection<HospitalDisplay> Hospitals { get; }
        public ReadOnlyObservableCollection<BranchDisplay> Branches { get; }
        public ReadOnlyObservableCollection<DepartmentDisplay> Departments { get; }
        public ReadOnlyObservableCollection<PositionDisplay> Positions { get; }

        public async Task LoadBranchesByHospitalIdAsync(string hospitalId)
        {
            ClearHospital();

            if (string.IsNullOrWhiteSpace(hospitalId))
                return;

            var branches = await _branchService.GetAllByHospitalIdAsync(hospitalId);

            if (branches == null)
                return;

            _branches.Load([.. branches.Value.Select(branch => new BranchDisplay(branch))], cleaning: true);
        }

        public async Task LoadDepartmentsByBranchIdAsync(string branchId)
        {
            ClearBranch();

            if (string.IsNullOrWhiteSpace(branchId))
                return;

            var departments = await _departmentService.GetAllByBranchIdAsync(branchId);

            if (departments == null)
                return;

            _departments.Load([.. departments.Value.Select(department => new DepartmentDisplay(department))], cleaning: true);
        }

        public async Task LoadPositionsByDepartmentIdAsync(string departmentId)
        {
            ClearDepartment();

            if (string.IsNullOrWhiteSpace(departmentId))
                return;

            var positions = await _positionFactory.Create(departmentId).GetAllAsync();

            if (positions == null) 
                return;

            _positions.Load([.. positions.Select(position => new PositionDisplay(position))], cleaning: true);
        }

        public void ClearHospital()
        {
            _branches.Clear();
            _departments.Clear();
            _positions.Clear();
        }

        public void ClearBranch()
        {
            _departments.Clear();
            _positions.Clear();
        }

        public void ClearDepartment()
        {
            _positions.Clear();
        }
    }
}
