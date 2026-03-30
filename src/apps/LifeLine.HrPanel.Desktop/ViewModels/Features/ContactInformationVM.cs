using LifeLine.HrPanel.Desktop.Models;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.HrPanel.Desktop.ViewModels.Features
{
    internal class ContactInformationVM : BaseEmployeeViewModel
    {
        public ContactInformationVM()
        {
            CreateNewContactInformation();
        }

        #region ContactInfo

        private string _personalPhone = null!;
        public string PersonalPhone
        {
            get => _personalPhone;
            set => SetProperty(ref _personalPhone, value);
        }

        private string _personalEmail = null!;
        public string PersonalEmail
        {
            get => _personalEmail;
            set => SetProperty(ref _personalEmail, value);
        }

        private string? _corporatePhone;
        public string? CorporatePhone
        {
            get => _corporatePhone;
            set => SetProperty(ref _corporatePhone, value);
        }

        private string? _corporateEmail;
        public string? CorporateEmail
        {
            get => _corporateEmail;
            set => SetProperty(ref _corporateEmail, value);
        }

        #endregion

        #region Address

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

        #endregion

        private ContactInformationDisplay _newContactInformation = null!;
        //public ContactInformationDisplay NewContactInformation
        //{
        //    get => _newContactInformation;
        //    private set => SetProperty(ref _newContactInformation, value);
        //}
        private void CreateNewContactInformation()
            => _newContactInformation = new(new ContactInformationResponse(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));
    
        public void ClearProperty()
        {
            PersonalPhone = string.Empty;
            PersonalEmail = string.Empty;
            CorporatePhone = string.Empty;
            CorporateEmail = string.Empty;

            PostalCode = string.Empty;
            Region = string.Empty;
            City = string.Empty;
            Street = string.Empty;
            Building = string.Empty;
            Apartment = string.Empty;
        }
    }
}
