using Shared.WPF.ViewModels.Abstract;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class DepartmentDisplay : BaseViewModel
    {
        private readonly DepartmentResponse _model;

        public DepartmentDisplay(DepartmentResponse model)
        {
            _model = model;

            Name = _model.Name;
            Description = _model.Description;
            Building = _model.Building;
        }

        public string DepartmentId => _model.Id;

        public string Name
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string? Description
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string Building
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public DepartmentResponse GetUnderlineModel() => _model;
    }
}
