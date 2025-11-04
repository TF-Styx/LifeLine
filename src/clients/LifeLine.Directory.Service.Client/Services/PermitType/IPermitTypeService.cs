using Shared.Contracts.Response.DirectoryService;
using Shared.Http.Base;

namespace LifeLine.Directory.Service.Client.Services.PermitType
{
    public interface IPermitTypeService : IPermitTypeReadOnlyService, IBaseWriteHttpService<AdmissionStatusResponse, string>;
}
