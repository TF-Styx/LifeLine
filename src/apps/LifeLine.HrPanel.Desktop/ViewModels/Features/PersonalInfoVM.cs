using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;
using System.Windows;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class PersonalInfoVM : BaseViewModel
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
    }
}
