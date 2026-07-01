using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;
using Shared.Domain.ValueObjects;
using Terminex.Common.Results;

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

        public async Task<Result> HasActiveAssignmentsToDepartmentAsync(DepartmentId departmentId, CancellationToken cancellationToken = default)
        {
            var exist = await _context.Employees.AnyAsync(x => x.Assignments.Any(x => x.DepartmentId == departmentId && !x.TerminationDate.HasValue), cancellationToken);

            return exist ? Result.Failure(Error.Exist("У данного отдела имеются назначения!")) : Result.Success();
        }

        public async Task<Result> HasActiveAssignmentsToPositionAsync(PositionId positionId, CancellationToken cancellationToken = default)
        {
            var exist = await _context.Employees.AnyAsync(x => x.Assignments.Any(x => x.PositionId == positionId && !x.TerminationDate.HasValue), cancellationToken);

            return exist ? Result.Failure(Error.Exist("У данной должности имеются назначения!")) : Result.Success();
        }
    }
}
