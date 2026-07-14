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
            if (Hospital?.Id == value.Id)
                return;

            Hospital = value;
            HospitalContextChanged?.Invoke(value.Id);

            ClearBranch();
            ClearDepartment();
            ClearPosition();
        }
        public void ClearHospital()
        {
            if (Hospital == null)
                return;

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
            if (Branch?.Id == value.Id)
                return;

            Branch = value;
            BranchContextChanged?.Invoke(value.Id);

            ClearDepartment();
            ClearPosition();
        }
        public void ClearBranch()
        {
            if (Branch == null)
                return;

            Branch = null;
            BranchContextChanged?.Invoke(null);

            ClearDepartment();
            ClearPosition();
        }

        public DepartmentResponse? Department { get; private set; }
        public event Action<string?>? DepartmentContextChanged;
        public void SetSelectedDepartment(DepartmentResponse value)
        {
            if (Department?.Id == value.Id)
                return;

            Department = value;
            DepartmentContextChanged?.Invoke(value.Id);

            ClearPosition();
        }
        public void ClearDepartment()
        {
            if (Department == null)
                return;

            Department = null;
            DepartmentContextChanged?.Invoke(null);

            ClearPosition();
        }

        public PositionResponse? Position { get; private set; }
        public event Action<string?>? PositionContextChanged;
        public void SetSelectedPosition(PositionResponse value)
        {
            if (Position?.Id == value.Id)
                return;

            Position = value;

            PositionContextChanged?.Invoke(value.Id);
        }
        public void ClearPosition()
        {
            if (Position == null)
                return;

            Position = null;

            PositionContextChanged?.Invoke(null);
        }
    }
}
