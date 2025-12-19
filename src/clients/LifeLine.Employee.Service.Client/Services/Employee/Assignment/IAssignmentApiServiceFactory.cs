namespace LifeLine.Employee.Service.Client.Services.Employee.Assignment
{
    public interface IAssignmentApiServiceFactory
    {
        IAssignmentService Create(string employeeId);
    }
}
