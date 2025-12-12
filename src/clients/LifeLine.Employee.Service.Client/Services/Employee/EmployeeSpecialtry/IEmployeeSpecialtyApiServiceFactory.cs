namespace LifeLine.Employee.Service.Client.Services.Employee.EmployeeSpecialtry
{
    public interface IEmployeeSpecialtyApiServiceFactory
    {
        IEmployeeSpecialtyService Create(string employeeId);
    }
}
