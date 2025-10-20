using LifeLine.Directory.Service.Domain.ValueObjects;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace LifeLine.Directory.Service.Domain.Models
{
    /// <summary>
    /// Тип документа
    /// </summary>
    public sealed class DocumentType : Aggregate<DocumentTypeId>
    {
        public DocumentTypeName DocumentTypeName { get; private set; } = null!;

        private DocumentType() { }
        public DocumentType(DocumentTypeId id, DocumentTypeName documentTypeName) : base(id) => DocumentTypeName = documentTypeName;

        public static DocumentType Create(string documentTypeName)
            => new DocumentType(DocumentTypeId.New(), DocumentTypeName.Create(documentTypeName));
    }
}
