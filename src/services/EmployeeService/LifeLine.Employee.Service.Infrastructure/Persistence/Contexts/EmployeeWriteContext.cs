using LifeLine.Employee.Service.Domain.Models;
using LifeLine.Employee.Service.Infrastructure.Persistence.Configurations.Write;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Contexts
{
    internal sealed class EmployeeWriteContext(DbContextOptions<EmployeeWriteContext> options) : DbContext(options), IWriteContext
    {
        public DbSet<Domain.Models.Employee> Employees { get; set; } = null!;
        public DbSet<ContactInformation> ContactInformations { get; set; } = null!;
        public DbSet<Gender> Genders { get; set; } = null!;

        public DbSet<Assignment> Assignments { get; set; } = null!;
        public DbSet<Contract> Contracts { get; set; } = null!;
        public DbSet<EmployeeType> EmployeeTypes { get; set; } = null!;

        public DbSet<WorkPermit> WorkPermits { get; set; } = null!;
        public DbSet<EducationDocument> EducationDocuments { get; set; } = null!;

        public DbSet<Specialty> Specialties { get; set; } = null!;
        public DbSet<EmployeeSpecialty> EmployeeSpecialties { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeWriteConfiguration());
            modelBuilder.ApplyConfiguration(new ContactInformationWriteConfiguration());
            modelBuilder.ApplyConfiguration(new GenderWriteConfiguration());

            modelBuilder.ApplyConfiguration(new AssignmentWriteConfiguration());
            modelBuilder.ApplyConfiguration(new ContractWriteConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeTypeWriteConfiguration());

            modelBuilder.ApplyConfiguration(new WorkPermitWriteConfiguration());
            modelBuilder.ApplyConfiguration(new EducationDocumentWriteConfiguration());

            modelBuilder.ApplyConfiguration(new SpecialtyWriteConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeSpecialtyWriteConfiguration());
        }
    }
}
