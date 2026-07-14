using Shared.Contracts.Response.DirectoryService;
using Shared.WPF.ViewModels.Abstract;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class HospitalDisplay : BaseViewModel
    {
        private readonly HospitalResponse _model;
        public HospitalDisplay(HospitalResponse model) 
        {
            _model = model;

            Name = _model.Name;
            Description = _model.Description;
            Phone = _model.Phone;
            Email = _model.Email;

            PostalCode = _model.Address.PostalCode;
            Region = _model.Address.Region;
            City = _model.Address.City;
            Street = _model.Address.Street;
            Building = _model.Address.Building;
            Apartment = _model.Address.Apartment;
        }

        public string HospitalId => _model.Id;

        public string Name
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string? Description
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string Phone
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string Email
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string PostalCode
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string Region
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string City
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string Street
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string? Building
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public string? Apartment
        {
            get => field;
            set => SetProperty(ref field, value);
        }

        public HospitalResponse GetUnderlineModel() => _model;
    }
}
