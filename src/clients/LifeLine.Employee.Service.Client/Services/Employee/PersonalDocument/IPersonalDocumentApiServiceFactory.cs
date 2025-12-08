namespace LifeLine.Employee.Service.Client.Services.Employee.PersonalDocument
{
    public interface IPersonalDocumentApiServiceFactory
    {
        IPersonalDocumentService Create(string employeeId);
    }
}
