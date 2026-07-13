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

        private ContactInformationDisplay _display = null!;
        public ContactInformationDisplay Display
        {
            get => _display;
            private set => SetProperty(ref _display, value);
        }
        private void CreateNewContactInformation()
            => Display = new(new ContactInformationResponse(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty));
    
        public void ClearProperty()
        {
            Display.PersonalPhone = string.Empty;
            Display.PersonalEmail = string.Empty;
            Display.CorporatePhone = string.Empty;
            Display.CorporateEmail = string.Empty;

            Display.PostalCode = string.Empty;
            Display.Region = string.Empty;
            Display.City = string.Empty;
            Display.Street = string.Empty;
            Display.Building = string.Empty;
            Display.Apartment = string.Empty;
        }
    }
}
