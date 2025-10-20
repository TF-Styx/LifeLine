using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.ValueObjects;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Configurations
{
    internal sealed class EducationLevelConfiguration : IEntityTypeConfiguration<EducationLevel>
    {
        public void Configure(EntityTypeBuilder<EducationLevel> builder)
        {
            builder.ToTable("EducationLevels");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(inDB => inDB.Value, outDB => EducationLevelId.Create(outDB));

            builder.Property(x => x.EducationLevelName)
                   .HasColumnName("Name")
                   .UseCollation(PostgresConstants.COLLATION_NAME)
                   .HasMaxLength(EducationLevelName.MAX_LENGTH)
                   .HasConversion(inDB => inDB.Value, outDB => EducationLevelName.Create(outDB));
        }
    }
}
