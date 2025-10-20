namespace Shared.Contracts.Request.EmployeeService.Specialty
{
    public sealed record CreateSpecialtyRequest(string SpecialtyName, string? Description);
}
