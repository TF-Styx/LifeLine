using Microsoft.Extensions.Options;
using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;
using System.Text.Json;

namespace LifeLine.Directory.Service.Client.Services.Hospital
{
    public sealed class HospitalService(HttpClient httpClient, IOptions<JsonSerializerOptions> options) 
        : BaseHttpService<HospitalResponse, string>(httpClient, "api/hospitals", options.Value), IHospitalService;
}
