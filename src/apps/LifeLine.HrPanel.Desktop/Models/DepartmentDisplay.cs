using Shared.WPF.ViewModels.Abstract;
using Shared.Contracts.Response.DirectoryService;

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

        private string _postalCode = model.Address.PostalCode;
        public string PostalCode
        {
            get => _postalCode;
            set => SetProperty(ref _postalCode, value);
        }

        private string _region = model.Address.Region;
        public string Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }

        private string _city = model.Address.City;
        public string City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        private string _street = model.Address.Street;
        public string Street
        {
            get => _street;
            set => SetProperty(ref _street, value);
        }

        private string _building = model.Address.Building;
        public string Building
        {
            get => _building;
            set => SetProperty(ref _building, value);
        }

        public DepartmentResponse GetUnderlineModel() => _model;
    }
}
