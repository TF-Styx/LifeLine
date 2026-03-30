namespace Shared.Contracts.Request.EmployeeService.EmployeeSpecialty
{
    public sealed record CreateManyEmployeeSpecialtiesRequest(List<CreateManyDataEmployeeSpecialtiesRequest> Specialties);
    public sealed record CreateManyDataEmployeeSpecialtiesRequest(string SpecialtyId);
}
