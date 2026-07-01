using Shared.Kernel.Primitives;
using Shared.Domain.ValueObjects;
using LifeLine.Directory.Service.Domain.ValueObjects;

namespace LifeLine.Directory.Service.Domain.Models
{
    public sealed class Hospital : Aggregate<HospitalId>
    {
        public DirectoryName Name { get; private set; } = null!;
        public Description? Description { get; private set; }
        public Phone Phone { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public Address Address { get; private set; } = null!;

        private Hospital() { }
        private Hospital(HospitalId id, DirectoryName name, Description? description, Phone phone, Email email, Address address) : base(id)
        {
            Name = name;
            Description = description;
            Phone = phone;
            Email = email;
            Address = address;
        }

        public static Hospital Create(string name, string? description, string phone, string email, Address address)
            => new Hospital
            (
                HospitalId.New(),
                DirectoryName.Create(name),
                !string.IsNullOrWhiteSpace(description) ? Description.Create(description) : null,
                Phone.Create(phone),
                Email.Create(email),
                address
            );

        public void UpdateName(DirectoryName name)
        {
            if (name != Name)
                Name = name;
        }

        public void UpdateDescription(Description? description)
        {
            if (description != Description)
                Description = description;
        }

        public void UpdatePhone(Phone phone)
        {
            if (phone != Phone)
                Phone = phone;
        }

        public void UpdateEmail(Email email)
        {
            if (email != Email)
                Email = email;
        }

        public void UpdateAddress(Address address)
        {
            if (address != Address)
                Address = address;
        }
    }
}
