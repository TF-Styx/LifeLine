namespace Shared.Contracts.Response.EmployeeService
{
    public sealed record EmployeeResponse(Guid Id, string Surname, string Name, string Patronymic, Guid GenderId, string GenderName);
}
