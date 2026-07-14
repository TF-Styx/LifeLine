using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Contexts
{
    internal sealed class DirectoryContext(DbContextOptions options) : DbContext(options), IDirectoryContext
    {
        public DbSet<Hospital> Hospitals { get; set; } = null!;
        public DbSet<Branch> Branches { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;
        public DbSet<Status> Statuses { get; set; } = null!;

        public DbSet<AdmissionStatus> AdmissionStatuses { get; set; } = null!;
        public DbSet<DocumentType> DocumentTypes { get; set; } = null!;
        public DbSet<EducationLevel> EducationLevels { get; set; } = null!;
        public DbSet<PermitType> PermitTypes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new HospitalConfiguration());
            modelBuilder.ApplyConfiguration(new BranchConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new PositionConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());

            modelBuilder.ApplyConfiguration(new AdmissionStatusConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EducationLevelConfiguration());
            modelBuilder.ApplyConfiguration(new PermitTypeConfiguration());

            modelBuilder.Entity<Hospital>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Branch>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Department>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Position>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
