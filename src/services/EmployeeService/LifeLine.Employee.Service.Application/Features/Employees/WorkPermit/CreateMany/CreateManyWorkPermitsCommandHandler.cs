using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.WorkPermit.CreateMany
{
    public sealed class CreateManyWorkPermitsCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<CreateManyWorkPermitsCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(CreateManyWorkPermitsCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            foreach (var item in request.WorkPermits)
                employee.AddWorkPermit
                    (
                        item.WorkPermitName,
                        item.DocumentSeries,
                        item.WorkPermitNumber,
                        item.ProtocolNumber,
                        item.SpecialtyName,
                        item.IssuingAuthority,
                        item.IssueDate,
                        item.ExpiryDate,
                        item.BucketName,
                        item.FileName,
                        item.PermitTypeId,
                        item.AdmissionStatusId
                    );

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
