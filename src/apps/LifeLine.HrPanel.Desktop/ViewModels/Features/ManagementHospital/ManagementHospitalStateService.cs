using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementHospital
{
    public sealed class ManagementHospitalStateService
    {
        public HospitalResponse? Hospital { get; private set; }
        public readonly IReadOnlyCollection<HospitalDisplay> Hospitals = [];
        public event Action<string?>? HospitalContextChanged;
        public void SetSelectedHospital(HospitalResponse value)
        {
            Hospital = value;
            HospitalContextChanged?.Invoke(value.Id);

            ClearBranch();
            ClearDepartment();
            ClearPosition();
        }
        public void ClearHospital()
        {
            Hospital = null;
            HospitalContextChanged?.Invoke(null);

            ClearBranch();
            ClearDepartment();
            ClearPosition();
        }

        public BranchResponse? Branch { get; private set; }
        public event Action<string?>? BranchContextChanged;
        public void SetSelectedBranch(BranchResponse value)
        {
            Branch = value;
            BranchContextChanged?.Invoke(value.Id);

            ClearDepartment();
            ClearPosition();
        }
        public void ClearBranch()
        {
            Branch = null;
            BranchContextChanged?.Invoke(null);

            ClearDepartment();
            ClearPosition();
        }

        public DepartmentResponse? Department { get; private set; }
        public event Action<string?>? DepartmentContextChanged;
        public void SetSelectedDepartment(DepartmentResponse value)
        {
            Department = value;
            DepartmentContextChanged?.Invoke(value.Id);

            ClearPosition();
        }
        public void ClearDepartment()
        {
            Department = null;
            DepartmentContextChanged?.Invoke(null);

            ClearPosition();
        }

        public PositionResponse? Position { get; private set; }
        public event Action<string?>? PositionContextChanged;
        public void SetSelectedPosition(PositionResponse value)
        {
            Position = value;

            PositionContextChanged?.Invoke(value.Id);
        }
        public void ClearPosition()
        {
            Position = null;

            PositionContextChanged?.Invoke(null);
        }
    }
}
