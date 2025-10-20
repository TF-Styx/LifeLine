using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.Employee.Service.Domain.ValueObjects.Shared;
using LifeLine.Employee.Service.Domain.ValueObjects.WorkPermits;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;
using Shared.Domain.Exceptions;

namespace LifeLine.Employee.Service.Domain.Models
{
    public sealed class WorkPermit : Entity<WorkPermitId>
    {
        public EmployeeId EmployeeId { get; private set; }
        public ProgramEducationName WorkPermitName { get; private set; } = null!;
        public DocumentSeries? DocumentSeries { get; private set; }
        public DocumentNumber WorkPermitNumber { get; private set; } = null!;
        public ProtocolNumber? ProtocolNumber { get; private set; }
        public SpecialtyName SpecialtyName { get; private set; } = null!;
        public IssuingAuthority IssuingAuthority { get; private set; } = null!;
        public DateTime IssueDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public ImageKey? FileKey { get; private set; }
        public PermitTypeId PermitTypeId { get; private set; }
        public AdmissionStatusId AdmissionStatusId { get; private set; }

        public Employee Employee { get; private set; } = null!;

        private WorkPermit() { }
        private WorkPermit
            (
                WorkPermitId id, 
                EmployeeId employeeId, 
                ProgramEducationName workPermitName, 
                DocumentNumber workPermitNumber, 
                SpecialtyName specialtyName, 
                IssuingAuthority issuingAuthority, 
                DateTime issueDate, 
                DateTime expiryDate,
                PermitTypeId permitTypeId,
                AdmissionStatusId admissionStatusId
            ) : base(id)
        {
            EmployeeId = employeeId;
            WorkPermitName = workPermitName;
            WorkPermitNumber = workPermitNumber;
            SpecialtyName = specialtyName;
            IssuingAuthority = issuingAuthority;
            IssueDate = issueDate;
            ExpiryDate = expiryDate;
            PermitTypeId = permitTypeId;
            AdmissionStatusId = admissionStatusId;
        }

        /// <exception cref="EmptyIdentifierException"></exception>        
        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="LengthException"></exception>
        public static WorkPermit Create
            (
                Guid employeeId,
                string workPermitName,
                string? documentSeries,
                string workPermitNumber,
                string? protocolNumber,
                string specialtyName,
                string issuingAuthority,
                DateTime issueDate,
                DateTime expiryDate,
                Guid permitTypeId,
                Guid admissionStatusId
            )
        {
            var workPermit = new WorkPermit
                (
                    WorkPermitId.New(),
                    EmployeeId.Create(employeeId),
                    ProgramEducationName.Create(workPermitName),
                    DocumentNumber.Create(workPermitNumber),
                    SpecialtyName.Create(specialtyName),
                    IssuingAuthority.Create(issuingAuthority),
                    issueDate, expiryDate,
                    PermitTypeId.Create(permitTypeId),
                    AdmissionStatusId.Create(admissionStatusId)
                );

            if (!string.IsNullOrWhiteSpace(documentSeries))
                workPermit.DocumentSeries = DocumentSeries.Create(documentSeries);

            if (!string.IsNullOrWhiteSpace(protocolNumber))
                workPermit.ProtocolNumber = ProtocolNumber.Create(protocolNumber);

            return workPermit;
        }
    }
}
