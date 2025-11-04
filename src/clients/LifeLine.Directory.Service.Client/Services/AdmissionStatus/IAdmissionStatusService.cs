using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.AdmissionStatus
{
    public interface IAdmissionStatusService : IAdmissionStatusReadOnlyService, IBaseWriteHttpService<AdmissionStatusResponse, string>;
}
