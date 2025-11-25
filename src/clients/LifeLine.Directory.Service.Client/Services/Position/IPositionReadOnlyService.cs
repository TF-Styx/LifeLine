using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.Position
{
    public interface IPositionReadOnlyService : IBaseReadHttpService<PositionResponse, string>
    {
        Task<List<PositionResponse>> GetAllPosition();
    }
}
