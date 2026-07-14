using LifeLine.Directory.Service.Domain.ValueObjects;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;
using Shared.Domain.Exceptions;

namespace LifeLine.Directory.Service.Domain.Models
{
    public sealed class Position : Entity<PositionId>
    {
        public DirectoryName Name { get; private set; } = null!;
        public Description? Description { get; private set; }
        public DepartmentId DepartmentId { get; private set; }
        public bool IsDeleted { get; private set; } = false;

        public Department Department { get; private set; } = null!;

        private Position() { }
        private Position(PositionId id, DirectoryName name, Description? description, DepartmentId departmentId) : base(id)
        {
            Name = name;
            Description = description;
            DepartmentId = departmentId;
        }

        /// <summary>
        /// Создание НОВОЙ должности
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="departmentId"></param>
        /// <exception cref="EmptyIdentifierException"></exception>
        /// <exception cref="LengthException"></exception>
        /// <returns cref="Position">НОВЫЙ объект Position</returns>
        internal static Position Create(string name, Description? description, Guid departmentId)
            => new Position
                (
                    PositionId.New(), 
                    DirectoryName.Create(name), 
                    description, 
                    DepartmentId.Create(departmentId)
                );

        /// <summary>
        /// Обновление имени должности
        /// </summary>
        /// <param name="name"></param>
        internal void UpdateName(DirectoryName name)
        {
            if (name != Name)
                Name = name;
        }

        /// <summary>
        /// Обновление описания должности
        /// </summary>
        /// <param name="description"></param>
        internal void UpdateDescription(Description? description)
        {
            if (description != Description)
                Description = description;
        }

        /// <summary>
        /// SoftDelete "Удаление"
        /// </summary>
        internal void Delete() => IsDeleted = true;

        /// <summary>
        /// Восстановление
        /// </summary>
        internal void Restore() => IsDeleted = false;
    }
}
