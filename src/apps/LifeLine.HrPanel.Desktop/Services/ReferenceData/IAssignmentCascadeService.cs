using LifeLine.HrPanel.Desktop.Models;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;

namespace LifeLine.HrPanel.Desktop.Services.ReferenceData
{
    public interface IAssignmentCascadeService : IAsyncInitializable
    {
        ReadOnlyObservableCollection<HospitalDisplay> Hospitals { get; }
        ReadOnlyObservableCollection<BranchDisplay> Branches { get; }
        ReadOnlyObservableCollection<DepartmentDisplay> Departments { get; }
        ReadOnlyObservableCollection<PositionDisplay> Positions { get; }

        Task LoadBranchesByHospitalIdAsync(string hospitalId);
        Task LoadDepartmentsByBranchIdAsync(string branchId);
        Task LoadPositionsByDepartmentIdAsync(string departmentId);

        void ClearHospital();
        void ClearBranch();
        void ClearDepartment();
    }
}
