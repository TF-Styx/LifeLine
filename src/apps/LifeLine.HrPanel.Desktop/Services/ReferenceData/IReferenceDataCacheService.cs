using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;
using System.Collections.ObjectModel;

namespace LifeLine.HrPanel.Desktop.Services.ReferenceData
{
    public interface IReferenceDataCacheService : IAsyncInitializable
    {
        ReadOnlyObservableCollection<GenderDisplay> Genders { get; }
        ReadOnlyObservableCollection<StatusDisplay> Statuses { get; }
        ReadOnlyObservableCollection<DocumentTypeDisplay> DocumentTypes { get; }
        ReadOnlyObservableCollection<PermitTypeDisplay> PermitTypes { get; }
        ReadOnlyObservableCollection<AdmissionStatusDisplay> AdmissionStatuses { get; }
        ReadOnlyObservableCollection<EducationLevelDisplay> EducationLevels { get; }
        ReadOnlyObservableCollection<EmployeeTypeDisplay> EmployeeTypes { get; }
        ReadOnlyObservableCollection<SpecialtyDisplay> Specialties { get; }
        ReadOnlyObservableCollection<ManagerDisplay> Managers { get; }
        ReadOnlyObservableCollection<HospitalDisplay> Hospitals { get; }
        ReadOnlyObservableCollection<BranchDisplay> Branches { get; }
        ReadOnlyObservableCollection<DepartmentDisplay> Departments { get; }
        ReadOnlyObservableCollection<PositionDisplay> Positions { get; }
    }
}
