using LifeLine.Employee.Service.Domain.ValueObjects.ContactInformation;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;
using Shared.Domain.Exceptions;

namespace LifeLine.Employee.Service.Domain.Models
{
    public sealed class ContactInformation : Entity<ContactInformationId>
    {
        public EmployeeId EmployeeId { get; private set; }
        public Phone PersonalPhone { get; private set; } = null!;
        public Phone? CorporatePhone { get; private set; }
        public Email PersonalEmail { get; private set; } = null!;
        public Email? CorporateEmail { get; private set; } = null!;
        public Address HomeAddress { get; private set; } = null!;

        public Employee Employee { get; private set; } = null!;

        private ContactInformation() { }
        private ContactInformation
            (
                ContactInformationId id,
                EmployeeId employeeId,
                Phone personalPhone,
                Phone? corporatePhone,
                Email personalEmail,
                Email? corporateEmail,
                Address homeAddress
            ) : base(id)
        {
            EmployeeId = employeeId;
            PersonalPhone = personalPhone;
            CorporatePhone = corporatePhone;
            PersonalEmail = personalEmail;
            CorporateEmail = corporateEmail;
            HomeAddress = homeAddress;
        }

        /// <exception cref="EmptyIdentifierException"></exception>
        /// <exception cref="PhoneNumberException"></exception>
        /// <exception cref="EmailAddressException"></exception>
        public static ContactInformation Create
            (
                Guid employeeId,
                string personalPhone,
                string? corporatePhone,
                string personalEmail,
                string? corporateEmail,
                Address homeAddress
            ) => new ContactInformation
                (
                    ContactInformationId.New(),
                    EmployeeId.Create(employeeId),
                    Phone.Create(personalPhone),
                    corporatePhone != null ? Phone.Create(corporatePhone) : Phone.Null,
                    Email.Create(personalEmail),
                    corporateEmail != null ? Email.Create(corporateEmail) : Email.Null,
                    homeAddress
                );

        internal void UpdatePersonalPhone(Phone phone)
        {
            if (phone != PersonalPhone)
                PersonalPhone = phone;
        }

        internal void UpdateCorporatePhone(Phone? phone)
        {
            if (phone !=  CorporatePhone)
                CorporatePhone = phone;
        }

        internal void UpdatePersonalEmail(Email email)
        {
            if (email != PersonalEmail)
                PersonalEmail = email;
        }

        internal void UpdateCorporateEmail(Email? email)
        {
            if (email != CorporateEmail)
                CorporateEmail = email;
        }

        internal void UpdateAddress(Address address) => HomeAddress = address;
    }
}
