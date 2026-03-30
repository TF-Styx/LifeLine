using Shared.Contracts.Request.EmployeeService.EmployeeSpecialty;
using Shared.Http.Interfaces;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.EmployeeSpecialtry
{
    public interface IEmployeeSpecialtyService : ICreateHttpService
    {
        Task<Result> CreateManyAsync(CreateManyEmployeeSpecialtiesRequest request);
        Task<Result> UpdateEmployeeSpecialtyAsync(UpdateEmployeeSpecialtyRequest request);
        Task<Result> DeleteEmployeeSpecialtyAsync(Guid specialtyId);
    }
}
