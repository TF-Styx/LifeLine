using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;

namespace LifeLine.Employee.Service.Client.Services.EmployeeType
{
    public interface IEmployeeTypeService : IEmployeeTypeReadOnlyService, IBaseWriteHttpService<EmployeeTypeResponse, string>;
}
