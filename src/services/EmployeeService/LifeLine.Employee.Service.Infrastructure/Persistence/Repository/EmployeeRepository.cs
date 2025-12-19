using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Repository
{
    public sealed class EmployeeRepository(IWriteContext context) : BaseRepository<Domain.Models.Employee, IWriteContext>(context), IEmployeeRepository
    {
        public async Task<Domain.Models.Employee?> GetByIdAsync(Guid id) 
            => await _context.Employees
                .Include(x => x.ContactInformation)
                .Include(x => x.EmployeeSpecialties)
                .Include(x => x.PersonalDocuments)
                .Include(x => x.EducationDocuments)
                .Include(x => x.WorkPermits)
                .Include(x => x.Assignments)
                .Include(x => x.Contracts)
                    .AsSplitQuery()
                        .FirstOrDefaultAsync(x => x.Id == id);
    }
}
