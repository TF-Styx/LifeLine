using LifeLine.Directory.Service.Domain.ValueObjects;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace LifeLine.Directory.Service.Domain.Models
{
    /// <summary>
    /// Уровень образования
    /// </summary>
    public sealed class EducationLevel : Aggregate<EducationLevelId>
    {
        public EducationLevelName EducationLevelName { get; private set; } = null!;

        private EducationLevel() { }
        private EducationLevel(EducationLevelId id, EducationLevelName educationLevelName) : base(id) => EducationLevelName = educationLevelName;

        public static EducationLevel Create(string educationLevelName)
            => new EducationLevel(EducationLevelId.New(), EducationLevelName.Create(educationLevelName));
    }
}
