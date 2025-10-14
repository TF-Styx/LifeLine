using LifeLine.Employee.Service.Domain.Models;
using Shared.Api.Infrastructure;

namespace LifeLine.EmployeeService.Application.Abstraction.Common.Repositories
{
    public interface IContractRepository : IBaseRepository<Contract>
    {
        Task<Contract?> GetByIdAsync(Guid id);
    }
}
