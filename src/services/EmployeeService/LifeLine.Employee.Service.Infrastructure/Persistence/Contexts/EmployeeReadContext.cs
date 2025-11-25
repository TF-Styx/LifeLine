using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Contexts
{
    public sealed class EmployeeReadContext(DbContextOptions<EmployeeReadContext> options) : DbContext(options), IReadContext
    {
        public DbSet<GenderView> GenderViews { get; set; } = null!;
        public DbSet<EmployeeAdminListItemView> EmployeeAdminListItemViews { get; set; } = null!;
        public DbSet<EmployeeTypeView> EmployeeTypeViews { get; set; } = null!;
        public DbSet<EmployeeHrItemView> EmployeeHrItemViews { get; set; } = null!;
        public DbSet<EmployeeFullDetailsView> EmployeeFullDetailsViews { get; set; } = null!;


        IQueryable<GenderView> IReadContext.GenderViews => this.GenderViews;
        IQueryable<EmployeeAdminListItemView> IReadContext.EmployeeAdminListItemViews => this.EmployeeAdminListItemViews;
        IQueryable<EmployeeTypeView> IReadContext.EmployeeTypeViews => this.EmployeeTypeViews;
        IQueryable<EmployeeHrItemView> IReadContext.EmployeeHrItemViews => this.EmployeeHrItemViews;
        IQueryable<EmployeeFullDetailsView> IReadContext.EmployeeFullDetailsViews => this.EmployeeFullDetailsViews;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EmployeeAdminListItemView>(builder =>
            {
                builder.ToTable("V_Employee_Admin_ListItem");
                builder.HasKey(x => x.Id);
            });

            modelBuilder.Entity<EmployeeTypeView>(builder =>
            {
                builder.ToTable("V_EmployeeTypes");
                builder.HasKey(x => x.Id);
            });

            modelBuilder.Entity<EmployeeHrItemView>(builder =>
            {
                builder.ToTable("V_Employee_Hr_Items");
                builder.HasKey(x => x.Id);
                builder.OwnsMany(x => x.Assignments, ownedBuilder => { ownedBuilder.ToJson(); });
            });

            modelBuilder.Entity<EmployeeFullDetailsView>(builder =>
            {
                builder.ToView("V_Employee_Full_Details");
                builder.HasKey(e => e.EmployeeId);

                builder.Property(e => e.Gender).HasColumnName("Gender").HasColumnType("jsonb");
                builder.Property(e => e.ContactInformation).HasColumnName("ContactInformation").HasColumnType("jsonb");
                builder.Property(e => e.Assignments).HasColumnName("Assignments").HasColumnType("jsonb");
                builder.Property(e => e.Contracts).HasColumnName("Contracts").HasColumnType("jsonb");
                builder.Property(e => e.EducationDocuments).HasColumnName("EducationDocuments").HasColumnType("jsonb");
                builder.Property(e => e.PersonalDocuments).HasColumnName("PersonalDocuments").HasColumnType("jsonb");
                builder.Property(e => e.Specialties).HasColumnName("Specialties").HasColumnType("jsonb");
                builder.Property(e => e.WorkPermits).HasColumnName("WorkPermits").HasColumnType("jsonb");
            });

            modelBuilder.Entity<GenderView>(builder =>
            {
                builder.ToView("V_Genders");
                builder.HasKey(x => x.Id);
            });

            base.OnModelCreating(modelBuilder);
        }

        IQueryable<TEntity> IBaseReadDbContext.Set<TEntity>()
        {
            throw new NotImplementedException();
        }
    }
}
