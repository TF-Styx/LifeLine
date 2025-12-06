using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class ContactInformationDisplay(ContactInformationResponse model) : BaseViewModel
    {
        private ContactInformationResponse _model = model;

        public string ContactInformationId => _model.Id;

        private string _personalPhone = model.PersonalPhone;
        public string PersonalPhone
        {
            get => _personalPhone;
            set => SetProperty(ref _personalPhone, value);
        }

        private string? _corporatePhone = model.CorporatePhone;
        public string? CorporatePhone
        {
            get => _corporatePhone;
            set => SetProperty(ref _corporatePhone, value);
        }

        private string _personalEmail = model.PersonalEmail;
        public string PersonalEmail
        {
            get => _personalEmail;
            set => SetProperty(ref _personalEmail, value);
        }

        private string? _corporateEmail = model.CorporateEmail;
        public string? CorporateEmail
        {
            get => _corporateEmail;
            set => SetProperty(ref _corporateEmail, value);
        }

        private string _postalCode = model.PostalCode;
        public string PostalCode
        {
            get => _postalCode;
            set => SetProperty(ref _postalCode, value);
        }

        private string _region = model.Region;
        public string Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }

        private string _city = model.City;
        public string City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        private string _street = model.Street;
        public string Street
        {
            get => _street;
            set => SetProperty(ref _street, value);
        }

        private string _building = model.Building;
        public string Building
        {
            get => _building;
            set => SetProperty(ref _building, value);
        }

        private string? _apartment = model.Apartment;
        public string? Apartment
        {
            get => _apartment;
            set => SetProperty(ref _apartment, value);
        }

        public void ReversChanches()
        {
            PersonalPhone = _model.PersonalPhone;
            CorporatePhone = _model.CorporatePhone;
            PersonalEmail = _model.PersonalEmail;
            CorporateEmail = _model.CorporateEmail;
            PostalCode = _model.PostalCode;
            Region = _model.Region;
            City = _model.City;
            Street = _model.Street;
            Building = _model.Building;
            Apartment = _model.Apartment;
        }

        public void CommitChanges()
        {
            _model = _model with
            {
                PersonalPhone = PersonalPhone,
                CorporatePhone = CorporatePhone,
                PersonalEmail = PersonalEmail,
                CorporateEmail = CorporateEmail,
                PostalCode = PostalCode,
                Region = Region,
                City = City,
                Street = Street,
                Building = Building,
                Apartment = Apartment
            };
        }
    }
}
