using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.CreateMany
{
    public sealed class CreateManyPersonalDocumentsCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<CreateManyPersonalDocumentsCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(CreateManyPersonalDocumentsCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            foreach (var item in request.PersonalDocuments)
                employee.AddPersonalDocument(item.DocumentTypeId, item.DocumentNumber, item.DocumentSeries, item.BucketName, item.FileName);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
