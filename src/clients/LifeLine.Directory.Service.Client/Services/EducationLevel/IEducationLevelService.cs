using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.EducationLevel
{
    public interface IEducationLevelService : IEducationLevelReadOnlyService, IBaseWriteHttpService<EducationLevelResponse, string>;
}