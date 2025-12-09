using LifeLine.Employee.Service.Domain.ValueObjects.EducationDocuments;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.Employee.Service.Domain.ValueObjects.Shared;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace LifeLine.Employee.Service.Domain.Models
{
    public sealed class EducationDocument : Entity<EducationDocumentId>
    {
        public EmployeeId EmployeeId { get; private set; }
        public EducationLevelId EducationLevelId { get; private set; }
        public DocumentTypeId DocumentTypeId { get; private set; }
        public DocumentNumber DocumentNumber { get; private set; } = null!;
        public DateTime IssuedDate { get; private set; }
        public IssuingAuthority OrganizationName { get; private set; } = null!;
        public QualificationAwardedName? QualificationAwardedName { get; private set; } = null!;
        public SpecialtyName? SpecialtyName { get; private set; } = null!;
        public ProgramEducationName? ProgramName { get; private set; } = null!;
        public Hours? TotalHours { get; private set; } = null!;

        public Employee Employee { get; private set; } = null!;

        private EducationDocument() { }
        private EducationDocument
            (
                EducationDocumentId id, 
                EmployeeId employeeId, 
                EducationLevelId educationLevelId,
                DocumentTypeId documentTypeId,
                DocumentNumber documentNumber,
                DateTime issuedDate,
                IssuingAuthority organizationName
            ) : base(id)
        {
            EmployeeId = employeeId;
            EducationLevelId = educationLevelId;
            DocumentTypeId = documentTypeId;
            DocumentNumber = documentNumber;
            IssuedDate = issuedDate;
            OrganizationName = organizationName;
        }

        public static EducationDocument Create
            (
                Guid employeeId,
                Guid educationLevelId,
                Guid documentTypeId,
                string documentNumber,
                DateTime issuedDate,
                string organizationName,
                string? qualificationAwardedName,
                string? specialtyName,
                string? programName,
                TimeSpan? totalHours
            )
        {
            var educationDocument = new EducationDocument
                (
                    EducationDocumentId.New(),
                    EmployeeId.Create(employeeId),
                    EducationLevelId.Create(educationLevelId),
                    DocumentTypeId.Create(documentTypeId),
                    DocumentNumber.Create(documentNumber),
                    issuedDate.ToUniversalTime(),
                    IssuingAuthority.Create(organizationName)
                );

            if (!string.IsNullOrWhiteSpace(qualificationAwardedName))
                educationDocument.QualificationAwardedName = QualificationAwardedName.Create(qualificationAwardedName);

            if (!string.IsNullOrWhiteSpace(specialtyName))
                educationDocument.SpecialtyName = SpecialtyName.Create(specialtyName);

            if (!string.IsNullOrWhiteSpace(programName))
                educationDocument.ProgramName = ProgramEducationName.Create(programName);

            if (totalHours.HasValue)
                educationDocument.TotalHours = Hours.Create(totalHours.Value.TotalHours);

            return educationDocument;
        }

        internal void UpdateEducationLevel(EducationLevelId educationLevelId)
        {
            if (educationLevelId != EducationLevelId)
                EducationLevelId = educationLevelId;
        }

        internal void UpdateDocumentType(DocumentTypeId documentTypeId)
        {
            if (documentTypeId != DocumentTypeId)
                DocumentTypeId = documentTypeId;
        }

        internal void UpdateDocumentNumber(DocumentNumber documentNumber)
        {
            if (documentNumber != DocumentNumber)
                DocumentNumber = documentNumber;
        }

        internal void UpdateIssuedDate(DateTime issuedDate)
        {
            if (issuedDate != IssuedDate)
                IssuedDate = issuedDate.ToUniversalTime();
        }

        internal void UpdateOrganizationName(IssuingAuthority issuingAuthority)
        {
            if (issuingAuthority != OrganizationName)
                OrganizationName = issuingAuthority;
        }

        internal void UpdateQualificationAwardedName(QualificationAwardedName? qualificationAwardedName)
        {
            if (qualificationAwardedName != QualificationAwardedName)
                QualificationAwardedName = qualificationAwardedName;
        }

        internal void UpdateSpecialtyName(SpecialtyName? specialtyName)
        {
            if (specialtyName != SpecialtyName)
                SpecialtyName = specialtyName;
        }

        internal void UpdateProgramName(ProgramEducationName? programEducationName)
        {
            if (programEducationName != ProgramName)
                ProgramName = programEducationName;
        }

        internal void UpdateTotalHours(Hours? totalHours)
        {
            if (totalHours != TotalHours)
                TotalHours = totalHours;
        }
    }
}
