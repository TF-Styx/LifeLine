using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Domain.ValueObjects.AddressVO;
using LifeLine.Directory.Service.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.ValueObjects;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Configurations
{
    internal sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(inDB => inDB.Value, outDB => DepartmentId.Create(outDB));

            builder.Property(x => x.Name)
                   .HasColumnName("Name")
                   .UseCollation(PostgresConstants.COLLATION_NAME)
                   .HasMaxLength(DirectoryName.MAX_LENGTH)
                   .HasConversion(inDB => inDB.Value, outDB => DirectoryName.Create(outDB));

            builder.Property(x => x.Description)
                   .HasColumnName("Description")
                   .UseCollation(PostgresConstants.COLLATION_NAME)
                   .HasMaxLength(Description.MAX_LENGTH)
                   .IsRequired(false)
                   .HasConversion(inDB => inDB != null ? inDB.Value : null, outDB => outDB != null ? Description.Create(outDB) : null);

            builder.Property(x => x.Building)
                   .HasColumnName("Building")
                   .UseCollation(PostgresConstants.COLLATION_NAME)
                   .HasMaxLength(Building.MAX_LENGTH)
                   .HasConversion(inDB => inDB.Value, outDB => Building.Create(outDB));

            builder.Property(x => x.BranchId)
                   .HasColumnName("BranchId")
                   .HasConversion(inDB => inDB.Value, outDB => BranchId.Create(outDB));

            builder.Property(x => x.IsDeleted)
                   .HasColumnName("IsDeleted")
                   .HasDefaultValue(false);

            builder.HasOne<Branch>().WithMany().HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Positions).WithOne(x => x.Department).HasForeignKey(x => x.DepartmentId).OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(x => x.Positions).HasField("_positions").UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
