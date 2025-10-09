using LifeLine.Directory.Service.Domain.ValueObjects;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace LifeLine.Directory.Service.Domain.Models
{
    public sealed class Department : Aggregate<DepartmentId>
    {
        public DirectoryName Name { get; private set; } = null!;
        public Description Description { get; private set; } = null!;
        public Address DepartmentAddress { get; private set; } = null!;

        private readonly List<Position> _positions = [];
        public IReadOnlyCollection<Position> Positions => _positions.AsReadOnly();

        private Department() { }
        private Department(DepartmentId id, DirectoryName name, Description description, Address departmentAddress) : base(id)
        {
            Name = name;
            Description = description;
            DepartmentAddress = departmentAddress;
        }

        public static Department Create(string name, string description, Address departmentAddress)
            => new Department(DepartmentId.New(), DirectoryName.Create(name), Description.Create(description), departmentAddress);

        public void AddPositions(string name, string description) => _positions.Add(Position.Create(name, description, this.Id));

        public void UpdateName(DirectoryName name)
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
