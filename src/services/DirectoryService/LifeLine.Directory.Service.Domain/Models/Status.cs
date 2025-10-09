using LifeLine.Directory.Service.Domain.ValueObjects;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace LifeLine.Directory.Service.Domain.Models
{
    public sealed class Status : Aggregate<StatusId>
    {
        public DirectoryName Name { get; private set; } = null!;
        public Description Description { get; private set; } = null!;

        private Status() { }
        private Status(StatusId id, DirectoryName name, Description description) : base(id)
        {
            Name = name;
            Description = description;
        }

        public static Status Create(string name, string description) 
            => new Status(StatusId.New(), DirectoryName.Create(name), Description.Create(description));

        public void UpdateStatusName(DirectoryName name)
        {
            if (name != Name)
                Name = name;
        }

        public void UpdateStatusDescription(Description description)
        {
            if (description != Description)
                Description = description;
        }
    }
}
