using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Domain.ValueObjects;
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
                   .HasConversion(inDB => inDB.Value, outDB => Description.Create(outDB));

            builder.Navigation(x => x.Positions).HasField("_positions").UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsOne(x => x.DepartmentAddress, addressBuilder =>
            {
                addressBuilder.Property(x => x.PostalCode).HasColumnName("PostalCode").IsRequired().HasMaxLength(Address.MAX_POSTAL_CODE_LENGTH);
                addressBuilder.Property(x => x.Region).HasColumnName("Region").IsRequired().HasMaxLength(Address.MAX_REGION_LENGTH);
                addressBuilder.Property(x => x.City).HasColumnName("City").IsRequired().HasMaxLength(Address.MAX_CITY_LENGTH);
                addressBuilder.Property(x => x.Street).HasColumnName("Street").IsRequired().HasMaxLength(Address.MAX_STREET_LENGTH);
                addressBuilder.Property(x => x.Building).HasColumnName("Building").IsRequired().HasMaxLength(Address.MAX_BUILDING_LENGTH);
                addressBuilder.Property(x => x.Apartment).HasColumnName("Apartment").IsRequired(false).HasMaxLength(Address.MAX_APARTMENT_LENGTH);
            });
        }
    }
}
