using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.Update
{
    public sealed class UpdateEducationDocumentCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<UpdateEducationDocumentCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdateEducationDocumentCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(Guid.Parse(request.EmployeeId));

            if (employee is null)
                return Error.NotFound("Пользователь не найден!");

            employee.UpdateEducationLevel(Guid.Parse(request.Id), Guid.Parse(request.EducationLevelId));
            employee.UpdateDocumentTypeED(Guid.Parse(request.Id), Guid.Parse(request.DocumentTypeId));
            employee.UpdateDocumentNumberED(Guid.Parse(request.Id), request.DocumentNumber);
            employee.UpdateIssuedDate(Guid.Parse(request.Id), request.IssuedDate);
            employee.UpdateOrganizationName(Guid.Parse(request.Id), request.OrganizationName);
            employee.UpdateQualificationAwardedName(Guid.Parse(request.Id), request.QualificationAwardedName);
            employee.UpdateSpecialtyName(Guid.Parse(request.Id), request.SpecialtyName);
            employee.UpdateProgramName(Guid.Parse(request.Id), request.ProgramName);
            employee.UpdateTotalHours(Guid.Parse(request.Id), request.TotalHours);
            employee.UpdateFileKeyEducationDocument(Guid.Parse(request.Id), request.BucketName, request.FileName);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
