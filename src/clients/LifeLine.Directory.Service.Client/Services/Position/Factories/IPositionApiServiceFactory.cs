namespace LifeLine.Directory.Service.Client.Services.Position.Factories
{
    public interface IPositionApiServiceFactory
    {
        IPositionService Create(string departmentId);
    }
}
