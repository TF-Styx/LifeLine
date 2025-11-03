using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.EducationLevel
{
    public interface IEducationLevelReadOnlyService : IBaseReadHttpService<EducationLevelResponse, string>;
}
