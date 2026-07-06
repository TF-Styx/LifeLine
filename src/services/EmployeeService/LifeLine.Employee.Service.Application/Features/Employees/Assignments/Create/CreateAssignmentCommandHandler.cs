using MediatR;
using Terminex.Common.Results;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.Create
{
    public sealed class CreateAssignmentCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<CreateAssignmentCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            var contract = employee.AddContract
                (
                    request.Contract.EmployeeTypeId,
                    request.Contract.ContractNumber,
                    request.Contract.StartDate,
                    request.Contract.EndDate,
                    request.Contract.Salary,
                    request.Contract.BucketName,
                    request.Contract.FileName
                );

            var assignmentId = employee.AddAssignment
                (
                    request.PositionId,
                    request.DepartmentId,
                    Guid.NewGuid(),
                    request.ManagerId,
                    request.HireDate,
                    request.TerminationDate,
                    request.StatusId,
                    contract.Id
                );
                
            await context.SaveChangesAsync(cancellationToken);

            return assignmentId;
        }
    }
}
