using Shared.Kernel.Primitives;
using Shared.Domain.ValueObjects;
using LifeLine.Directory.Service.Domain.ValueObjects;

namespace LifeLine.Directory.Service.Domain.Models
{
    public sealed class Branch : Aggregate<BranchId>
    {
        public DirectoryName Name { get; private set; } = null!;
        public Description? Description { get; private set; }
        public Phone Phone { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public HospitalId HospitalId { get; private set; }
        public Address Address { get; private set; } = null!;

        private Branch() { }
        private Branch(BranchId id, DirectoryName name, Description? description, Phone phone, Email email, HospitalId hospitalId, Address address) : base(id)
        {
            Name = name;
            Description = description;
            Phone = phone;
            Email = email;
            HospitalId = hospitalId;
            Address = address;
        }

        public static Branch Create(string name, string? description, string phone, string email, Guid hospitalId, Address address)
            => new Branch
            (
                BranchId.New(),
                DirectoryName.Create(name),
                !string.IsNullOrWhiteSpace(description) ? Description.Create(description) : null,
                Phone.Create(phone),
                Email.Create(email),
                HospitalId.Create(hospitalId),
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

        public void UpdateHospitalId(HospitalId hospitalId)
        {
            if (hospitalId != HospitalId)
                HospitalId = hospitalId;
        }

        public void UpdateAddress(Address address)
        {
            if (address != Address)
                Address = address;
        }
    }
}
