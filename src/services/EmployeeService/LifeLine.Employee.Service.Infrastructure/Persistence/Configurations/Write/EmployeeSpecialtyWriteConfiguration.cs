using LifeLine.Employee.Service.Domain.Models;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.Employee.Service.Domain.ValueObjects.Specialties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class EmployeeSpecialtyWriteConfiguration : IEntityTypeConfiguration<EmployeeSpecialty>
    {
        public void Configure(EntityTypeBuilder<EmployeeSpecialty> builder)
        {
            builder.ToTable("EmployeeSpecialties");
            builder.HasKey(x => new { x.EmployeeId, x.SpecialtyId });

            builder.Property(x => x.EmployeeId)
                   .HasColumnName("EmployeeId")
                   .HasConversion(inDB => inDB.Value, outDB => EmployeeId.Create(outDB));

            builder.Property(x => x.SpecialtyId)
                   .HasColumnName("SpecialtyId")
                   .HasConversion(inDB => inDB.Value, outDB => SpecialtyId.Create(outDB));

            builder.HasOne<Specialty>().WithMany().HasForeignKey(x => x.SpecialtyId).IsRequired(true).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
