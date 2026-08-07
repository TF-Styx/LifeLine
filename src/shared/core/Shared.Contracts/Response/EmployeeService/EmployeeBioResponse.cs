namespace Shared.Contracts.Response.EmployeeService
{
    public sealed record EmployeeBioResponse
        (
            string EmployeeId, 
            string Surname, 
            string Name, 
            string? Patronymic, 
            string GenderId, 
            string? PersonalPhotoKey, 
            ContactInformationResponse? ContactInformation,
            List<string>? SpecialtyIds
        );
}
