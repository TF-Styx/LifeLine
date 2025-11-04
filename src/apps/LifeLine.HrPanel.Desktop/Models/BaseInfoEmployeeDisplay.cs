using Shared.Contracts.Response.DirectoryService;
using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class BaseInfoEmployeeDisplay : BaseViewModel
    {
        #region Employees

        private string _surname = null!;
        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        private string _name = null!;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string? _patronymic;
        public string? Patronymic
        {
            get => _patronymic;
            set => SetProperty(ref _patronymic, value);
        }

        #endregion

        #region Genders

        private GenderResponse? _gender;
        public GenderResponse? Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        #endregion

        #region DocumentType

        private string _number = null!;
        public string Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        private string? _series;
        public string? Series
        {
            get => _series;
            set => SetProperty(ref _series, value);
        }

        //SelectedDocumentType
        private DocumentTypeResponse _documentType = null!;
        public DocumentTypeResponse DocumentType
        {
            get => _documentType;
            set => SetProperty(ref _documentType, value);
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

        #region Personal / Corporate

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
    }
}
