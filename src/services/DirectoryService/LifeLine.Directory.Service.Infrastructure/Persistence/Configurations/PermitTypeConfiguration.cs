using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.ValueObjects;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Configurations
{
    internal sealed class PermitTypeConfiguration : IEntityTypeConfiguration<PermitType>
    {
        public void Configure(EntityTypeBuilder<PermitType> builder)
        {
            builder.ToTable("PermitTypes");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(inDB => inDB.Value, outDB => PermitTypeId.Create(outDB));

            builder.Property(x => x.PermitTypeName)
                   .HasColumnName("Name")
                   .UseCollation(PostgresConstants.COLLATION_NAME)
                   .HasMaxLength(PermitTypeName.MAX_LENGTH)
                   .HasConversion(inDB => inDB.Value, outDB => PermitTypeName.Create(outDB));
        }
    }
}
