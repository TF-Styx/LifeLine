using LifeLine.EmployeeService.Application.Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction
{
    public interface IReadContext : IBaseReadDbContext
    {
        IQueryable<GenderView> GenderViews { get; }
        IQueryable<EmployeeAdminListItemView> EmployeeAdminListItemViews { get; }
    }
}
