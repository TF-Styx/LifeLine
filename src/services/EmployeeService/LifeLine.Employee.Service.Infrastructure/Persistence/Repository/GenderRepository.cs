using LifeLine.Employee.Service.Domain.Models;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Repository
{
    public sealed class GenderRepository(IWriteContext writeContext) : BaseRepository<Gender, IWriteContext>(writeContext), IGenderRepository
    {
        public async Task<bool> ExistNameAsync(string name) => await _context.Genders.AnyAsync(x => x.Name == name);

        public async Task<Gender?> GetByIdAsync(Guid id) => await _context.Genders.FirstOrDefaultAsync(x => x.Id == id);
    }
}
