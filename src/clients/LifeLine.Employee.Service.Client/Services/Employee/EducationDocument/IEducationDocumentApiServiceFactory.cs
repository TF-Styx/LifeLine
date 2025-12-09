namespace LifeLine.Employee.Service.Client.Services.Employee.EducationDocument
{
    public interface IEducationDocumentApiServiceFactory
    {
        IEducationDocumentService Create(string employeeId);
    }
}
