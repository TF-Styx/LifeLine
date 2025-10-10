using LifeLine.Directory.Service.Domain.ValueObjects;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;
using Shared.Domain.Exceptions;

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

        /// <summary>
        /// Создание НОВОГО статуса
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <exception cref="EmptyIdentifierException"></exception>
        /// <exception cref="LengthException"></exception>
        /// <returns cref="Status">НОВЫЙ объект Status</returns>
        public static Status Create(string name, string description) 
            => new Status(StatusId.New(), DirectoryName.Create(name), Description.Create(description));

        /// <summary>
        /// Обновление имени статуса
        /// </summary>
        /// <param name="name"></param>
        public void UpdateStatusName(DirectoryName name)
        {
            if (name != Name)
                Name = name;
        }

        /// <summary>
        /// Обновление описания статуса
        /// </summary>
        /// <param name="description"></param>
        public void UpdateStatusDescription(Description description)
        {
            if (description != Description)
                Description = description;
        }
    }
}
