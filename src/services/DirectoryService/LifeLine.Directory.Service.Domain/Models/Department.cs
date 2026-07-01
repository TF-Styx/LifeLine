using Shared.Kernel.Guard;
using Shared.Kernel.Primitives;
using Shared.Domain.Exceptions;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Guard.Extensions;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Domain.ValueObjects.AddressVO;

namespace LifeLine.Directory.Service.Domain.Models
{
    public sealed class Department : Aggregate<DepartmentId>
    {
        public DirectoryName Name { get; private set; } = null!;
        public Description? Description { get; private set; }
        public Building Building { get; private set; } = null!;
        public BranchId BranchId { get; private set; }

        private readonly List<Position> _positions = [];
        public IReadOnlyCollection<Position> Positions => _positions.AsReadOnly();


        private Department() { }
        private Department(DepartmentId id, DirectoryName name, Description? description, Building building, BranchId branchId) : base(id)
        {
            Name = name;
            Description = description;
            Building = building;
            BranchId = branchId;
        }

        /// <summary>
        /// Создание НОВОГО отдела
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="departmentAddress"></param>
        /// <exception cref="EmptyIdentifierException"></exception>
        /// <exception cref="LengthException"></exception>
        /// <returns cref="Department">НОВЫЙ объект Department</returns>
        public static Department Create(string name, string? description, string building, Guid branchId)
            => new Department
            (
                DepartmentId.New(), 
                DirectoryName.Create(name), 
                !string.IsNullOrWhiteSpace(description) ? Description.Create(description) : null, 
                Building.Create(building), 
                BranchId.Create(branchId)
            );

        /// <summary>
        /// Обновление имени отдела
        /// </summary>
        /// <param name="name"></param>
        public void UpdateName(DirectoryName name)
        {
            if (name != Name)
                Name = name;
        }

        /// <summary>
        /// Обновление описания отдела
        /// </summary>
        /// <param name="description"></param>
        public void UpdateDescription(Description? description)
        {
            if (description != Description) 
                Description = description;
        }

        /// <summary>
        /// Обновление строения отдела
        /// </summary>
        /// <param name="building"></param>
        public void UpdateBuilding(Building building)
        {
            if (building != Building)
                Building = building;
        }


        public void UpdateBranchId(BranchId branchId)
        {
            if (branchId != BranchId)
                BranchId = branchId;
        }

        #region Position

        /// <summary>
        /// Добавление НОВЫХ должностей
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <exception cref="DuplicateException"></exception>
        public void AddPositions(string name, string? description)
        {
            GuardException.Against.That(_positions.Any(x => x.Name == name), () => new DuplicateException("Должность с таким именем уже существует!"));

            _positions.Add(Position.Create(name, !string.IsNullOrWhiteSpace(description) ? Description.Create(description) : null, this.Id));
        }

        /// <summary>
        /// Обновление имени должности
        /// </summary>
        /// <param name="positionId"></param>
        /// <param name="name"></param>
        /// <exception cref="DuplicateException"></exception>
        /// <exception cref="RecordMissingException"></exception>
        public void UpdatePositionName(Guid positionId, string name)
        {
            GuardException.Against.That(_positions.Any(x => x.Id != positionId && x.Name == name), () => new DuplicateException("Должность с таким именем уже существует!"));

            var position = _positions.FirstOrDefault(x => x.Id == positionId);

            GuardException.Against.That(position == null, () => new RecordMissingException("Должность не найдена!"));

            position!.UpdateName(DirectoryName.Create(name));
        }

        /// <summary>
        /// Обновление описание должности
        /// </summary>
        /// <param name="positionId"></param>
        /// <param name="description"></param>
        /// <exception cref="RecordMissingException"></exception>
        public void UpdatePositionDescription(Guid positionId, string? description)
        {
            var position = _positions.FirstOrDefault(x => x.Id == positionId);

            GuardException.Against.That(position == null, () => new RecordMissingException("Должность не найдена!"));

            position!.UpdateDescription(!string.IsNullOrWhiteSpace(description) ? Description.Create(description) : null);
        }

        /// <summary>
        /// Удаление должностей
        /// </summary>
        /// <param name="positionId"></param>
        /// <exception cref="RecordMissingException"></exception>
        public void RemovePosition(Guid positionId)
        {
            var position = _positions.FirstOrDefault(x => x.Id == positionId);

            GuardException.Against.That(position == null, () => new RecordMissingException("Должность не найдена!"));

            _positions.Remove(position!);
        }

        #endregion
    }
}
