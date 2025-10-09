using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.ValueObjects;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Configurations
{
    internal sealed class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.ToTable("Positions");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(inDB => inDB.Value, outDB => PositionId.Create(outDB));

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

            builder.Property(x => x.DepartmentId)
                   .HasColumnName("DepartmentId")
                   .HasConversion(inDB => inDB.Value, outDB => DepartmentId.Create(outDB));

            builder.HasOne(x => x.Department).WithMany(x => x.Positions).HasForeignKey(x => x.DepartmentId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
