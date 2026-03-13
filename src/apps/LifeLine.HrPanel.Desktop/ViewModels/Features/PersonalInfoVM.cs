using LifeLine.HrPanel.Desktop.Models;
using Shared.WPF.ViewModels.Abstract;

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

        public GenderDisplay? SelectedGender
        {
            get => field;
            set
            {
                SetProperty(ref field, value);
            }
        }
    }
}
