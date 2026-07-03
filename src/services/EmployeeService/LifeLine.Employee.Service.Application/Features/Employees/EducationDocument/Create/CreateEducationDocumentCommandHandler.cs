using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.Create
{
    public sealed class CreateEducationDocumentCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<CreateEducationDocumentCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(CreateEducationDocumentCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            employee.AddEducationDocument
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
                null, 
                null
            );

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
