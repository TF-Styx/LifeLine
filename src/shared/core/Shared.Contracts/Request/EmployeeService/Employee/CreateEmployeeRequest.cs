namespace Shared.Contracts.Request.EmployeeService.Employee
{
    public sealed record CreateEmployeeRequest(string Surname, string Name, string Patronymic, string GenderId);
}
