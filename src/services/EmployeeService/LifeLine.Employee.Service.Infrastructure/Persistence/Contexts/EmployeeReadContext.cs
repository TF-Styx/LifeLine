using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Contexts
{
    public sealed class EmployeeReadContext(DbContextOptions<EmployeeReadContext> options) : DbContext(options), IReadContext
    {
        public DbSet<GenderView> GenderViews { get; set; } = null!;

        IQueryable<GenderView> IReadContext.GenderViews => this.GenderViews;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
