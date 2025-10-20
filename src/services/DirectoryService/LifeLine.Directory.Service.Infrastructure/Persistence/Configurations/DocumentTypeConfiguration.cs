using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.ValueObjects;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Configurations
{
    internal sealed class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentType>
    {
        public void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            builder.ToTable("DocumentTypes");
            builder.HasKey(t => t.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(inDB => inDB.Value, outDB => DocumentTypeId.Create(outDB));

            builder.Property(x => x.DocumentTypeName)
                   .HasColumnName("Name")
                   .UseCollation(PostgresConstants.COLLATION_NAME)
                   .HasMaxLength(DocumentTypeName.MAX_LENGTH)
                   .HasConversion(inDB => inDB.Value, outDB => DocumentTypeName.Create(outDB));
        }
    }
}
