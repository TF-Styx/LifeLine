using Shared.Contracts.Request.EmployeeService.EmployeeSpecialty;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.EmployeeSpecialtry
{
    public interface IEmployeeSpecialtyService
    {
        Task<Result> UpdateEmployeeSpecialtyAsync(UpdateEmployeeSpecialtyRequest request);
        Task<Result> DeleteEmployeeSpecialtyAsync(Guid specialtyId);
    }
}
