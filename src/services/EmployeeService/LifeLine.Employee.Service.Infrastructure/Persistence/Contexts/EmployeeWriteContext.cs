using LifeLine.Employee.Service.Domain.Models;
using LifeLine.Employee.Service.Infrastructure.Persistence.Configurations.Write;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Contexts
{
    internal sealed class EmployeeWriteContext(DbContextOptions<EmployeeWriteContext> options) : DbContext(options), IWriteContext
    {
        public DbSet<Domain.Models.Employee> Employees { get; set; } = null!;
        public DbSet<Gender> Genders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EmployeeWriteConfiguration());
            modelBuilder.ApplyConfiguration(new GenderWriteConfiguration());
        }
    }
}
