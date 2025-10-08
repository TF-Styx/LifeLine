namespace Shared.Contracts.Request.EmployeeService.Employee
{
    public sealed record UpdateEmployeeRequest(string Surname, string Name, string Patronymic, string GenderId);
}
