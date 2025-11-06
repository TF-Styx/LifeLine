using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;

namespace LifeLine.Employee.Service.Client.Services.EmployeeType
{
    public interface IEmployeeTypeReadOnlyService : IBaseReadHttpService<EmployeeTypeResponse, string>;
}
