using Shared.Contracts.Request.EmployeeService.EmployeeSpecialty;
using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Client.Services.Employee.EmployeeSpecialtry
{
    public interface IEmployeeSpecialtyService : IBaseHttpService<SpecialtyResponse, string>
    {
        Task<Result> CreateManyAsync(CreateManyEmployeeSpecialtiesRequest request);
        Task<Result> UpdateEmployeeSpecialtyAsync(UpdateEmployeeSpecialtyRequest request);
        Task<Result> DeleteEmployeeSpecialtyAsync(string specialtyId);
    }
}
