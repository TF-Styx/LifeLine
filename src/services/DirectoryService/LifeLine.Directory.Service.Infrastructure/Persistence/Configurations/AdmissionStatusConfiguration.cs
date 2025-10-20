using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.ValueObjects;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Configurations
{
    internal sealed class AdmissionStatusConfiguration : IEntityTypeConfiguration<AdmissionStatus>
    {
        public void Configure(EntityTypeBuilder<AdmissionStatus> builder)
        {
            builder.ToTable("AdmissionStatuses");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(inDB => inDB.Value, outDB => AdmissionStatusId.Create(outDB));

            builder.Property(x => x.AdmissionStatusName)
                   .HasColumnName("Name")
                   .UseCollation(PostgresConstants.COLLATION_NAME)
                   .HasMaxLength(AdmissionStatusName.MAX_LENGTH)
                   .HasConversion(inDB => inDB.Value, outDB => AdmissionStatusName.Create(outDB));
        }
    }
}
