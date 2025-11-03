using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.EducationLevel
{
    public sealed class EducationLevelService(HttpClient httpClient) : BaseHttpService<EducationLevelResponse, string>(httpClient, "api/education-levels"), IEducationLevelService;
}
