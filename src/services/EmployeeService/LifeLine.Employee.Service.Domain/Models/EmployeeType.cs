using LifeLine.Employee.Service.Domain.ValueObjects.EmployeeType;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;
using Shared.Domain.Exceptions;

namespace LifeLine.Employee.Service.Domain.Models
{
    public sealed class EmployeeType : Aggregate<EmployeeTypeId>
    {
        public EmployeeTypeName Name { get; private set; } = null!;
        public Description Description { get; private set; } = null!;

        private EmployeeType() { }
        private EmployeeType(EmployeeTypeId id, EmployeeTypeName name, Description description) : base(id)
        {
            Name = name;
            Description = description;
        }

        /// <exception cref="EmptyIdentifierException"></exception>
        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static EmployeeType Create(string name, string description)
            => new EmployeeType(EmployeeTypeId.New(), EmployeeTypeName.Create(name), Description.Create(description));

        public void UpdateName(EmployeeTypeName name)
        {
            if (name != Name)
                Name = name;
        }

        public void UpdateDescription(Description description)
        {
            if (description != Description) 
                Description = description;
        }
    }
}
