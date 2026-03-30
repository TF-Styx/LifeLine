using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class PersonalInfoVM : BaseEmployeeViewModel
    {
        public string? Surname
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public string? Name
        {
            get => field;
            set => SetProperty(ref field, value);
        }
        public string? Patronymic
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public GenderResponse? Gender
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public void ClearProperty()
        {
            Surname = string.Empty;
            Name = string.Empty;
            Patronymic = string.Empty;
            Gender = null;
        }
    }
}
