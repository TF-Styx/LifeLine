using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class SpecialtyDisplay(SpecialtyResponse model) : BaseViewModel
    {
        private readonly SpecialtyResponse _model = model;

        public string SpecialtyId => _model.Id;

        private string _specialtyName = model.Name;
        public string SpecialtyName
        {
            get => _specialtyName;
            set => SetProperty(ref _specialtyName, value);
        }

        private string? _description = model.Description;
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
    }
}
