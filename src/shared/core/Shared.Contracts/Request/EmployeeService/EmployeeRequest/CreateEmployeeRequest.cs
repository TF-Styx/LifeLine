namespace Shared.Contracts.Request.EmployeeService.EmployeeRequest
{
    public sealed record CreateEmployeeRequest(string Surname, string Name, string Patronymic, string GenderId);
}
