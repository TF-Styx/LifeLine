using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class GenderDisplay(GenderResponse model) : BaseViewModel
    {
        private GenderResponse _model = model;

        public string GenderId => _model.Id;

        private string _genderName = model.Name;
        public string GenderName
        {
            get => _genderName;
            set => SetProperty(ref _genderName, value);
        }

        public void RevertChanges() => GenderName = _model.Name;

        public void CommitChanges()
        {
            _model = _model with
            {
                Name = GenderName
            };
        }
    }
}
