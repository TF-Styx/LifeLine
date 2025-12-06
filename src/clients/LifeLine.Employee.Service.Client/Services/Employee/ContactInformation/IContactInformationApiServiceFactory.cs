namespace LifeLine.Employee.Service.Client.Services.Employee.ContactInformation
{
    public interface IContactInformationApiServiceFactory
    {
        IContactInformationService Create(string employeeId);
    }
}
