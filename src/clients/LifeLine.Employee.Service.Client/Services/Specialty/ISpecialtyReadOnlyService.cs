using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;

namespace LifeLine.Employee.Service.Client.Services.Specialty
{
    public interface ISpecialtyReadOnlyService : IBaseReadHttpService<SpecialtyResponse, string>;
}
