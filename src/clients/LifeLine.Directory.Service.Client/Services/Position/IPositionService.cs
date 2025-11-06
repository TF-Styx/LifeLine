using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.Position
{
    public interface IPositionService : IPositionReadOnlyService, IBaseWriteHttpService<PositionResponse, string>;
}
