using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal sealed class AddressVM : BaseViewModel
    {
        private string _postalCode = null!;
        public string PostalCode
        {
            get => _postalCode;
            set => SetProperty(ref _postalCode, value);
        }

        private string _region = null!;
        public string Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }

        private string _city = null!;
        public string City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        private string _street = null!;
        public string Street
        {
            get => _street;
            set => SetProperty(ref _street, value);
        }

        private string _building = null!;
        public string Building
        {
            get => _building;
            set => SetProperty(ref _building, value);
        }

        private string? _apartment;
        public string? Apartment
        {
            get => _apartment;
            set => SetProperty(ref _apartment, value);
        }
    }
}
