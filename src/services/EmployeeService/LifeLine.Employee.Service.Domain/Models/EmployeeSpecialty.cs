using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.Employee.Service.Domain.ValueObjects.Specialties;
using Shared.Kernel.Primitives;

namespace LifeLine.Employee.Service.Domain.Models
{
    public sealed class EmployeeSpecialty : IEntity
    {
        public EmployeeId EmployeeId { get; private set; }
        public SpecialtyId SpecialtyId { get; private set; }

        public Employee Employee { get; private set; } = null!;

        private EmployeeSpecialty() { }
        private EmployeeSpecialty(EmployeeId employeeId, SpecialtyId specialtyId)
        {
            EmployeeId = employeeId;
            SpecialtyId = specialtyId;
        }

        internal static EmployeeSpecialty Create(Guid employeeId, Guid specialtyId)
            => new EmployeeSpecialty(EmployeeId.Create(employeeId), SpecialtyId.Create(specialtyId));

        internal void UpdateSpecialty(SpecialtyId specialtyId)
        {
            if (specialtyId != SpecialtyId)
                SpecialtyId = specialtyId;
        }
    }
}
