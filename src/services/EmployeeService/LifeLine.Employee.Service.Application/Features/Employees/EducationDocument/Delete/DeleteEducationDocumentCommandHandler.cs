using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.Delete
{
    public sealed class DeleteEducationDocumentCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<DeleteEducationDocumentCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeleteEducationDocumentCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            employee.DeleteEducationDocument(request.EducationDocumentId);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
