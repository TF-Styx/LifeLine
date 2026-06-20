using Shared.Contracts.Request.DirectoryService.Position;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Client.Services.Position
{
    public interface IPositionService : IPositionReadOnlyService, IBaseWriteHttpService<PositionResponse, string>
    {
        Task<Result> UpdateAsync(string positionId, UpdatePositionRequest request);
    }
}
