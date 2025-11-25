using Shared.Contracts.Response.EmployeeService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class EmployeeDetailsDisplay(EmployeeFullDetailsResponse model) : BaseViewModel
    {
        private readonly EmployeeFullDetailsResponse _model = model;

        public string EmployeeId => _model.EmployeeId.ToString();

        private string _surname = model.Surname;
        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        private string _name = model.Name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string? _patronymic = model.Patronymic;
        public string? Patronymic
        {
            get => _patronymic;
            set => SetProperty(ref _patronymic, value);
        }

        private DateTime _dateEntry = model.DateEntry;
        public DateTime DateEntry
        {
            get => _dateEntry;
            set => SetProperty(ref _dateEntry, value);
        }

        private double _rating = model.Rating;
        public double Rating
        {
            get => _rating;
            set => SetProperty(ref _rating, value);
        }

        private string? _avatar = model.Avatar;
        public string? Avatar
        {
            get => _avatar;
            set => SetProperty(ref _avatar, value);
        }

        private string? _genderName = model.Gender.GenderName;
        public string? Gender
        {
            get => _genderName;
            set => SetProperty(ref _genderName, value);
        }

        private string? _personalPhone = model.ContactInformation?.PersonalPhone;
        public string? PersonalPhone
        {
            get => _personalPhone;
            set => SetProperty(ref _personalPhone, value);
        }

        private string? _corporatePhone = model.ContactInformation?.CorporatePhone;
        public string? CorporatePhone
        {
            get => _corporatePhone;
            set => SetProperty(ref _corporatePhone, value);
        }

        private string? _personalEmail = model.ContactInformation?.PersonalEmail;
        public string? PersonalEmail
        {
            get => _personalEmail;
            set => SetProperty(ref _personalEmail, value);
        }

        private string? _corporateEmail = model.ContactInformation?.CorporateEmail;
        public string? CorporateEmail
        {
            get => _corporateEmail;
            set => SetProperty(ref _corporateEmail, value);
        }

        private string? _postalCode = model.ContactInformation?.PostalCode;
        public string? PostalCode
        {
            get => _postalCode;
            set => SetProperty(ref _postalCode, value);
        }

        private string? _region = model.ContactInformation?.Region;
        public string? Region
        {
            get => _region;
            set => SetProperty(ref _region, value);
        }

        private string? _city = model.ContactInformation?.City;
        public string? City
        {
            get => _city;
            set => SetProperty(ref _city, value);
        }

        private string? _street = model.ContactInformation?.Street;
        public string? Street
        {
            get => _street;
            set => SetProperty(ref _street, value);
        }

        private string? _building = model.ContactInformation?.Building;
        public string? Building
        {
            get => _building;
            set => SetProperty(ref _building, value);
        }

        private string? _apartment = model.ContactInformation?.Apartment;
        public string? Apartment
        {
            get => _apartment;
            set => SetProperty(ref _apartment, value);
        }
    }
}
