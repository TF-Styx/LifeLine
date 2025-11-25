using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class AdmissionStatusDisplay(AdmissionStatusResponse model) : BaseViewModel
    {
        private readonly AdmissionStatusResponse _model = model;

        public string Id => _model.Id.ToString();

        private string _name = model.Name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}
