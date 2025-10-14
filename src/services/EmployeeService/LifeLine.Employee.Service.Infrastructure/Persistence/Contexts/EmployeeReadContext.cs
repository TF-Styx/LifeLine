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

        IQueryable<GenderView> IReadContext.GenderViews => this.GenderViews;
        IQueryable<EmployeeAdminListItemView> IReadContext.EmployeeAdminListItemViews => this.EmployeeAdminListItemViews;
        IQueryable<EmployeeTypeView> IReadContext.EmployeeTypeViews => this.EmployeeTypeViews;

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
