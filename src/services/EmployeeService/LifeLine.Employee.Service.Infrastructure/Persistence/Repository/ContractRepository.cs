using LifeLine.Employee.Service.Domain.Models;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Employee.Service.Infrastructure.Persistence.Repository
{
    public sealed class ContractRepository(IWriteContext context) : BaseRepository<Contract, IWriteContext>(context), IContractRepository
    {
        public async Task<Contract?> GetByIdAsync(Guid id)
            => await _context.Contracts.FirstOrDefaultAsync(x => x.Id == id);
    }
}
