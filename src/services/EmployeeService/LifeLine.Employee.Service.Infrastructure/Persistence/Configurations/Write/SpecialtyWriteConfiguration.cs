using LifeLine.Employee.Service.Domain.Models;
using LifeLine.Employee.Service.Domain.ValueObjects.Shared;
using LifeLine.Employee.Service.Domain.ValueObjects.Specialties;
using LifeLine.Employee.Service.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.ValueObjects;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class SpecialtyWriteConfiguration : IEntityTypeConfiguration<Specialty>
    {
        public void Configure(EntityTypeBuilder<Specialty> builder)
        {
            builder.ToTable("Specialties");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(inDB => inDB.Value, outDB => SpecialtyId.Create(outDB));

            builder.Property(x => x.SpecialtyName)
                   .HasColumnName("Name")
                   .UseCollation(PostgresConstants.COLLATION_NAME)
                   .HasMaxLength(SpecialtyName.MAX_LENGTH)
                   .HasConversion(inDB => inDB.Value, outDB => SpecialtyName.Create(outDB));

            builder.Property(x => x.Description)
                   .HasColumnName("Description")
                   .UseCollation(PostgresConstants.COLLATION_NAME)
                   .HasMaxLength(Description.MAX_LENGTH)
                   .HasConversion(inDB => inDB != null ? inDB.Value : null, outDB => outDB != null ? Description.Create(outDB) : null);
        }
    }
}
