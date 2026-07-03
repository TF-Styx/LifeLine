using Microsoft.Extensions.Options;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using System.Text.Json;

namespace LifeLine.Directory.Service.Client.Services.EducationLevel
{
    public sealed class EducationLevelService(HttpClient httpClient, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<EducationLevelResponse, string>(httpClient, "api/education-levels", options.Value), IEducationLevelService;
}
