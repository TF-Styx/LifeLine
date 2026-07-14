using Shared.WPF.ViewModels.Abstract;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.HrPanel.Desktop.Models
{
    public sealed class BranchDisplay : BaseViewModel
    {
        private readonly BranchResponse _model;

        public BranchDisplay(BranchResponse model)
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

        public string BranchId => _model.Id;

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

        public string HospitalId => _model.HospitalId;

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

        public BranchResponse GetUnderlineModel() => _model;
    }
}
