using LifeLine.Employee.Service.Domain.Models;
using LifeLine.Employee.Service.Domain.ValueObjects.ContactInformation;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.ValueObjects;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class ContactInformationWriteConfiguration : IEntityTypeConfiguration<ContactInformation>
    {
        public void Configure(EntityTypeBuilder<ContactInformation> builder)
        {
            builder.ToTable("ContactInformations");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(inDB => inDB.Value, outDB => ContactInformationId.Create(outDB));

            //builder.Property(x => x.EmployeeId)
            //       .HasColumnName("EmployeeId")
            //       .ValueGeneratedNever()
            //       .HasConversion(inDB => inDB.Value, outDB => EmployeeId.Create(outDB));

            builder.Property(x => x.PersonalPhone)
                   .HasColumnName("PersonalPhone")
                   .HasConversion(inDB => inDB.Value, outDB => Phone.Create(outDB));

            builder.Property(x => x.CorporatePhone)
                   .HasColumnName("CorporatePhone")
                   .IsRequired(false)
                   .HasConversion(inDB => inDB != null ? inDB.Value : null, outDB => outDB != null ? Phone.Create(outDB) : null);

            builder.Property(x => x.PersonalEmail)
                   .HasColumnName("PersonalEmail")
                   .HasConversion(inDB => inDB.Value, outDB => Email.Create(outDB));

            builder.Property(x => x.CorporateEmail)
                   .HasColumnName("CorporateEmail")
                   .IsRequired(false)
                   .HasConversion(inDB => inDB != null ? inDB.Value : null, outDB => outDB != null ? Email.Create(outDB) : null);

            builder.OwnsOne(x => x.HomeAddress, addressBuilder =>
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
