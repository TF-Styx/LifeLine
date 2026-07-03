using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalDocuments.Delete
{
    public sealed class DeletePersonalDocuemtnCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<DeletePersonalDocuemtnCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeletePersonalDocuemtnCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee is null)
                return Error.NotFound("Пользователь не найден!");

            employee.DeletePersonalDocument(request.PersonalDocumentId);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
