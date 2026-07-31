using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features.ManagementEmployee
{
    public class ManagementEmployeeStateService
    {
        public EmployeeHrItemResponse? EmployeeHr { get; private set; }
        public event Action<string?>? EmployeeContextChanged;

        public void SetSelectedEmployee(EmployeeHrItemResponse value)
        {
            if (EmployeeHr?.Id == value.Id)
                return;

            EmployeeHr = value;
            EmployeeContextChanged?.Invoke(value.Id);
        }

        public void UpdateEmployeeData(EmployeeHrItemResponse response)
        {
            EmployeeHr = response;
            EmployeeContextChanged?.Invoke(response.Id);
        }

        public void ClearEmployee()
        {
            if (EmployeeHr == null)
                return;

            EmployeeHr = null;
            EmployeeContextChanged?.Invoke(null);
        }
    }
}
