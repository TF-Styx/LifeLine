using LifeLine.Employee.Service.Domain.Models;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Repository
{
    public sealed class AssignmentRepository(IWriteContext context) : BaseRepository<Assignment, IWriteContext>(context), IAssignmentRepository
    {
        public async Task<Assignment?> GetByIdAsync(Guid id)
            => await _context.Assignments.FirstOrDefaultAsync(x => x.Id == id);
    }
}
