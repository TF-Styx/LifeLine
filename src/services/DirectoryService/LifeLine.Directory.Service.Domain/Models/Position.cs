using LifeLine.Directory.Service.Domain.ValueObjects;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace LifeLine.Directory.Service.Domain.Models
{
    public sealed class Position : Entity<PositionId>
    {
        public DirectoryName Name { get; private set; } = null!;
        public Description Description { get; private set; } = null!;
        public DepartmentId DepartmentId { get; private set; }

        public Department Department { get; private set; } = null!;

        private Position() { }
        private Position(PositionId id, DirectoryName name, Description description, DepartmentId departmentId) : base(id)
        {
            Name = name;
            Description = description;
            DepartmentId = departmentId;
        }

        internal static Position Create(string name, string description, Guid departmentId)
            => new Position(PositionId.New(), DirectoryName.Create(name), Description.Create(description), DepartmentId.Create(departmentId));

        internal void UpdateName(DirectoryName name)
        {
            if (name != Name)
                Name = name;
        }

        internal void UpdateDescription(Description description)
        {
            if (description != Description)
                Description = description;
        }
    }
}
