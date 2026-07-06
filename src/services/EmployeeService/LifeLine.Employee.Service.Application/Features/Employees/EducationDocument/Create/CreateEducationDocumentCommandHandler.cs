using MediatR;
using Terminex.Common.Results;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.Create
{
    public sealed class CreateEducationDocumentCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<CreateEducationDocumentCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateEducationDocumentCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            var educationDocumentId = employee.AddEducationDocument
            (
                request.EducationLevelId,
                request.DocumentTypeId,
                request.DocumentNumber,
                request.IssuedDate,
                request.OrganizationName,
                request.QualificationAwardedName,
                request.SpecialtyName,
                request.ProgramName,
                request.TotalHours,
                request.BucketName,
                request.FileName
            );

            await context.SaveChangesAsync(cancellationToken);

            return educationDocumentId;
        }
    }
}
