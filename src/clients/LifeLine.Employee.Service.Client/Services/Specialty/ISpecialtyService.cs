using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;

namespace LifeLine.Employee.Service.Client.Services.Specialty
{
    public interface ISpecialtyService : ISpecialtyReadOnlyService, IBaseWriteHttpService<SpecialtyResponse, string>;
}
