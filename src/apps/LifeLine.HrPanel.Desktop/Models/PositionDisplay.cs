using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class PositionDisplay(PositionResponse model) : BaseViewModel
    {
        private readonly PositionResponse _model = model;

        public string PositionId => _model.Id;

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

        public PositionResponse GetUnderlineModel() => _model;
    }
}
