using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Repository
{
    public sealed class DepartmentRepository(IDirectoryContext context) : BaseRepository<Department, IDirectoryContext>(context), IDepartmentRepository
    {
        public async Task<Department?> GetByIdAsync(Guid id)
            => await _context.Departments
                .Include(x => x.Positions)
                    .FirstOrDefaultAsync(x => x.Id == id);
    }
}
