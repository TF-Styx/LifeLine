using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class ManagerDisplay(EmployeeResponse model) : BaseViewModel
    {
        private readonly EmployeeResponse _model = model;

        public string Id => _model.Id.ToString();

        private string _surname = model.Surname;
        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        private string _name = model.Name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _patronymic = model.Patronymic;
        public string Patronymic
        {
            get => _patronymic;
            set => SetProperty(ref _patronymic, value);
        }
    }
}
