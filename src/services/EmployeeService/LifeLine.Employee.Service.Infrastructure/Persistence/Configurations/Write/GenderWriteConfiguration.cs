using LifeLine.Employee.Service.Domain.Models;
using LifeLine.Employee.Service.Domain.ValueObjects.Genders;
using LifeLine.Employee.Service.Infrastructure.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class GenderWriteConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.ToTable("Genders");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(inDB => inDB.Value, outDB => GenderId.Create(outDB));

            builder.Property(x => x.Name)
                   .HasColumnName("Name")
                   .IsRequired()
                   .HasMaxLength(GenderName.MAX_LENGTH)
                   .UseCollation(PostgresConstants.COLLATION_NAME)
                   .HasConversion(inDB => inDB.Value, outDB => GenderName.Create(outDB));
        }
    }
}
