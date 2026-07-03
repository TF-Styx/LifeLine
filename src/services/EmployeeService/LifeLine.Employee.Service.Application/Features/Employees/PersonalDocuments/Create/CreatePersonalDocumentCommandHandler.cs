using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Create
{
    public sealed class CreatePersonalDocumentCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<CreatePersonalDocumentCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(CreatePersonalDocumentCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            employee.AddPersonalDocument(request.DocumentTypeId, request.DocumentNumber, request.DocumentSeries, null, null);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
