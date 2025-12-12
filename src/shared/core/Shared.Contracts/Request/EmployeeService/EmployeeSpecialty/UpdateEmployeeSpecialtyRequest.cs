namespace Shared.Contracts.Request.EmployeeService.EmployeeSpecialty
{
    public sealed record UpdateEmployeeSpecialtyRequest(string SpecialtyIdOld, string SpecialtyIdNew);
}
