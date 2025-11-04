using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.AdmissionStatus
{
    public sealed class AdmissionStatusService(HttpClient httpClient) : BaseHttpService<AdmissionStatusResponse, string>(httpClient, "api/admission-statuses"), IAdmissionStatusService;
}
