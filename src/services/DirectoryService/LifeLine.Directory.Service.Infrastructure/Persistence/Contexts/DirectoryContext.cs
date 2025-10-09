using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Contexts
{
    internal sealed class DirectoryContext(DbContextOptions options) : DbContext(options), IDirectoryContext
    {
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new PositionConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
        }
    }
}
