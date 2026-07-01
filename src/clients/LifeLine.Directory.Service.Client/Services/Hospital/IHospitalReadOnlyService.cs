using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.Hospital
{
    public interface IHospitalReadOnlyService : IBaseReadHttpService<HospitalResponse, string>;
}
