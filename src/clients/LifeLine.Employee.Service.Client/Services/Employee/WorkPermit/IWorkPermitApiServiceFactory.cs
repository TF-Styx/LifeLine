namespace LifeLine.Employee.Service.Client.Services.Employee.WorkPermit
{
    public interface IWorkPermitApiServiceFactory
    {
        IWorkPermitService Create(string employeeId);
    }
}