using Shared.Contracts.Response.EmployeeService;
using Shared.Http.Base;

namespace LifeLine.Employee.Service.Client.Services.Gender
{
    public interface IGenderReadOnlyService : IBaseReadHttpService<GenderResponse, string>;
}
