using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal class BaseEmployeeViewModel : BaseViewModel
    {
        public string? EmployeeId
        {
            get => field;
            set => SetProperty(ref field, value);
        }
    }
}
