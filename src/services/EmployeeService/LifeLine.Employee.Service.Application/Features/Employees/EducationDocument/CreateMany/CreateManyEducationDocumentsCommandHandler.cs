using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.CreateMany
{
    public sealed class CreateManyEducationDocumentsCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<CreateManyEducationDocumentsCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(CreateManyEducationDocumentsCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            foreach (var item in request.EducationDocuments)
                employee.AddEducationDocument
                    (
                        item.EducationLevelId,
                        item.DocumentTypeId,
                        item.DocumentNumber,
                        item.IssuedDate,
                        item.OrganizationName,
                        item.QualificationAwardedName,
                        item.SpecialtyName,
                        item.ProgramName,
                        item.TotalHours,
                        item.BucketName,
                        item.FileName
                    );

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
