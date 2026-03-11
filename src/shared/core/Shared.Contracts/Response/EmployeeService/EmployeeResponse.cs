namespace Shared.Contracts.Response.EmployeeService
{
    public sealed record EmployeeResponse(string Id, string Surname, string Name, string Patronymic, string GenderId, string GenderName);
}
