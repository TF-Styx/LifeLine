using LifeLine.Employee.Service.Domain.Models;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Repository
{
    public sealed class SpecialtyRepository(IWriteContext context) : BaseRepository<Specialty, IWriteContext>(context), ISpecialtyRepository
    {
        public async Task<Specialty?> GetByIdAsync(Guid id)
            => await _context.Specialties.FirstOrDefaultAsync(x => x.Id == id);
    }
}
