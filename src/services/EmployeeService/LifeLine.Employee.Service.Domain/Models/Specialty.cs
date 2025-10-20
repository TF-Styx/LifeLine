using LifeLine.Employee.Service.Domain.ValueObjects.Shared;
using LifeLine.Employee.Service.Domain.ValueObjects.Specialties;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace LifeLine.Employee.Service.Domain.Models
{
    public sealed class Specialty : Aggregate<SpecialtyId>
    {
        public SpecialtyName SpecialtyName { get; private set; } = null!;
        public Description? Description { get; private set; }

        private Specialty() { }
        private Specialty(SpecialtyId id, SpecialtyName specialtyName, Description? description) : base(id)
        {
            SpecialtyName = specialtyName;
            Description = description;
        }

        public static Specialty Create(string specialtyName, string? description)
            => new Specialty(SpecialtyId.New(), SpecialtyName.Create(specialtyName), description != null ? Description.Create(description) : Description.Null);
    }
}
