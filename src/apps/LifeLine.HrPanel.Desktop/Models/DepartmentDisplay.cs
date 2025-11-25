using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class DepartmentDisplay(DepartmentResponse model) : BaseViewModel
    {
        private readonly DepartmentResponse _model = model;

        public string Id => _model.Id.ToString();

        private string _name = model.Name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description = model.Description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }
    }
}
