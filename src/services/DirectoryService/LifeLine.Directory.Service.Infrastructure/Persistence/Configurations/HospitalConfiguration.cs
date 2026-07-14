using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.ValueObjects;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Configurations
{
    internal sealed class HospitalConfiguration : IEntityTypeConfiguration<Hospital>
    {
        public void Configure(EntityTypeBuilder<Hospital> builder)
        {
            builder.ToTable("Hospitals");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(inDB => inDB.Value, outDB => HospitalId.Create(outDB));

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

            builder.Property(x => x.Phone)
                   .HasColumnName("Phone")
                   .HasConversion(inDB => inDB.Value, outDB => Phone.Create(outDB));

            builder.Property(x => x.Email)
                   .HasColumnName("Email")
                   .HasConversion(inDB => inDB.Value, outDB => Email.Create(outDB));

            builder.OwnsOne(x => x.Address, addressBuilder =>
            {
                addressBuilder.Property(x => x.PostalCode).HasColumnName("PostalCode").IsRequired().HasMaxLength(Address.MAX_POSTAL_CODE_LENGTH);
                addressBuilder.Property(x => x.Region).HasColumnName("Region").IsRequired().HasMaxLength(Address.MAX_REGION_LENGTH);
                addressBuilder.Property(x => x.City).HasColumnName("City").IsRequired().HasMaxLength(Address.MAX_CITY_LENGTH);
                addressBuilder.Property(x => x.Street).HasColumnName("Street").IsRequired().HasMaxLength(Address.MAX_STREET_LENGTH);
                addressBuilder.Property(x => x.Building).HasColumnName("Building").IsRequired(false).HasMaxLength(Address.MAX_BUILDING_LENGTH);
                addressBuilder.Property(x => x.Apartment).HasColumnName("Apartment").IsRequired(false).HasMaxLength(Address.MAX_APARTMENT_LENGTH);
            });

            builder.Property(x => x.IsDeleted)
                   .HasColumnName("IsDeleted")
                   .HasDefaultValue(false);
        }
    }
}
