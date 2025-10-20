using LifeLine.Directory.Service.Domain.ValueObjects;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace LifeLine.Directory.Service.Domain.Models
{
    /// <summary>
    /// Статус сертификата
    /// </summary>
    public sealed class AdmissionStatus : Aggregate<AdmissionStatusId>
    {
        public AdmissionStatusName AdmissionStatusName { get; private set; } = null!;

        private AdmissionStatus() { }
        private AdmissionStatus(AdmissionStatusId id, AdmissionStatusName admissionStatusName) : base(id) => AdmissionStatusName = admissionStatusName;

        public static AdmissionStatus Create(string admissionStatusName) 
            => new AdmissionStatus(AdmissionStatusId.New(), AdmissionStatusName.Create(admissionStatusName));
    }
}
