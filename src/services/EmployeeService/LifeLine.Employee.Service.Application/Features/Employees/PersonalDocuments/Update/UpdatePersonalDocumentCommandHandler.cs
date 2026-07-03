using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Update
{
    public sealed class UpdatePersonalDocumentCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<UpdatePersonalDocumentCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdatePersonalDocumentCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(Guid.Parse(request.EmployeeId));

            if (employee is null)
                return Error.NotFound("Пользователь не найден!");

            employee.UpdateDocumentTypePD(Guid.Parse(request.Id), Guid.Parse(request.DocumentTypeId));
            employee.UpdateDocumentNumberPD(Guid.Parse(request.Id), request.DocumentNumber);
            employee.UpdateDocumentSeries(Guid.Parse(request.Id), request.DocumentSeries);
            employee.UpdateFileKeyPersonalDocument(Guid.Parse(request.Id), request.BucketName, request.FileName);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
