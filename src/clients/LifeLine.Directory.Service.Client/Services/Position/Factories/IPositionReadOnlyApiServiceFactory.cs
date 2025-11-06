namespace LifeLine.Directory.Service.Client.Services.Position.Factories
{
    public interface IPositionReadOnlyApiServiceFactory
    {
        IPositionReadOnlyService Create(string departmentId);
    }
}
