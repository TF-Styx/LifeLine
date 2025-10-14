using LifeLine.Employee.Service.Domain.Models;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Repository
{
    public sealed class EmployeeTypeRepository(IWriteContext context) : BaseRepository<EmployeeType, IWriteContext>(context), IEmployeeTypeRepository
    {
        public async Task<EmployeeType?> GetByIdAsync(Guid id)
            => await _context.EmployeeTypes.FirstOrDefaultAsync(x => x.Id == id);
    }
}
