namespace Shared.Contracts.Request.EmployeeService.EmployeeRequest
{
    public sealed record UpdateEmployeeRequest(string Surname, string Name, string Patronymic, string GenderId);
}
