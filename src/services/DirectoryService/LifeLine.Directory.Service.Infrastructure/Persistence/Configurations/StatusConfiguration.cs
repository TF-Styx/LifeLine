using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.ValueObjects;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Configurations
{
    internal sealed class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Statuses");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(inDB => inDB.Value, outDB => StatusId.Create(outDB));

            builder.Property(x => x.Name)
                   .HasColumnName("Name")
                   .UseCollation(PostgresConstants.COLLATION_NAME)
                   .HasMaxLength(DirectoryName.MAX_LENGTH)
                   .HasConversion(inDB => inDB.Value, outDB => DirectoryName.Create(outDB));

            builder.Property(x => x.Description)
                   .HasColumnName("Description")
                   .UseCollation(PostgresConstants.COLLATION_NAME)
                   .HasMaxLength(Description.MAX_LENGTH)
                   .HasConversion(inDB => inDB.Value, outDB => Description.Create(outDB));
        }
    }
}
