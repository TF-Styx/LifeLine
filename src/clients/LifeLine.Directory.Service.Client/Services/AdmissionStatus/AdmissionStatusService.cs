using Microsoft.Extensions.Options;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using System.Text.Json;

namespace LifeLine.Directory.Service.Client.Services.AdmissionStatus
{
    public sealed class AdmissionStatusService(HttpClient httpClient, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<AdmissionStatusResponse, string>(httpClient, "api/admission-statuses", options.Value), IAdmissionStatusService;
}
