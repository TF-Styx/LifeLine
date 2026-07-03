using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.Create
{
    public sealed class CreateWorkPermitCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<CreateWorkPermitCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(CreateWorkPermitCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            employee.AddWorkPermit
            (
                request.WorkPermitName,
                request.DocumentSeries,
                request.WorkPermitNumber,
                request.ProtocolNumber,
                request.SpecialtyName,
                request.IssuingAuthority,
                request.IssueDate,
                request.ExpiryDate,
                null,
                null,
                request.PermitTypeId,
                request.AdmissionStatusId
            );

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
