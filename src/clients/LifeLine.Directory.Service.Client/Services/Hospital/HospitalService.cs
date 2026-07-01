using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.Hospital
{
    public sealed class HospitalService(HttpClient httpClient) : BaseHttpService<HospitalResponse, string>(httpClient, "api/hospitals"), IHospitalService;
}
