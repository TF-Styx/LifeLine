using LifeLine.Employee.Service.Domain.Models;
using LifeLine.Employee.Service.Domain.ValueObjects.Assignments;
using LifeLine.Employee.Service.Domain.ValueObjects.Contracts;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.ValueObjects;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Configurations.Write
{
    internal sealed class AssignmentWriteConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable("Assignments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedNever()
                   .HasConversion(inDB => inDB.Value, outDB => AssignmentId.Create(outDB));

            builder.Property(x => x.EmployeeId)
                   .HasColumnName("EmployeeId")
                   .HasConversion(inDB => inDB.Value, outDB => EmployeeId.Create(outDB));

            builder.Property(x => x.PositionId)
                   .HasColumnName("PositionId")
                   .HasConversion(inDB => inDB.Value, outDB => PositionId.Create(outDB));

            builder.Property(x => x.DepartmentId)
                   .HasColumnName("DepartmentId")
                   .HasConversion(inDB => inDB.Value, outDB => DepartmentId.Create(outDB));

            builder.Property(x => x.ManagerId)
                   .HasColumnName("ManagerId")
                   .IsRequired(false)
                   .HasConversion(inDB => inDB != null ? inDB.Value : (Guid?)null, outDB => outDB != null ? EmployeeId.Create(outDB.Value) : EmployeeId.Null);

            builder.Property(x => x.HireDate)
                   .HasColumnName("HireDate");

            builder.Property(x => x.TerminationDate)
                   .HasColumnName("TerminationDate")
                   .IsRequired(false);

            builder.Property(x => x.StatusId)
                   .HasColumnName("StatusId")
                   .HasConversion(inDB => inDB.Value, outDB => StatusId.Create(outDB));

            builder.Property(x => x.ContractId)
                   .HasColumnName("ContractId")
                   .HasConversion(inDB => inDB.Value, outDB => ContractId.Create(outDB));

            builder.HasOne<Contract>().WithOne().HasForeignKey<Assignment>(assignment => assignment.ContractId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<Domain.Models.Employee>().WithMany().HasForeignKey(assignment => assignment.ManagerId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
