using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.ViewModels.Abstract;
using System.Reflection;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class PermitTypeDisplay(PermitTypeResponse model) : BaseViewModel
    {
        private readonly PermitTypeResponse _model = model;

        public string Id => _model.Id.ToString();

        private string _name = model.Name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}
