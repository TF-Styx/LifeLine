using LifeLine.Directory.Service.Domain.ValueObjects;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace LifeLine.Directory.Service.Domain.Models
{
    /// <summary>
    /// тип документа (сертификат или аккредитация)
    /// </summary>
    public sealed class PermitType : Aggregate<PermitTypeId>
    {
        public PermitTypeName PermitTypeName { get; private set; } = null!;

        private PermitType() { }
        private PermitType(PermitTypeId id, PermitTypeName permitTypeName) : base(id) => PermitTypeName = permitTypeName;

        public static PermitType Create(string permitTypeName) 
            => new PermitType(PermitTypeId.New(), PermitTypeName.Create(permitTypeName));
    }
}
