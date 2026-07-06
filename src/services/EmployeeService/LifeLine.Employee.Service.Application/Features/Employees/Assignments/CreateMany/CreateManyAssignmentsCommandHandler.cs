using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.CreateMany
{
    public sealed class CreateManyAssignmentsCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<CreateManyAssignmentsCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(CreateManyAssignmentsCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            foreach (var assignmentData in request.Assignments)
            {
                var contract = employee.AddContract
                    (
                        assignmentData.Contracts.EmployeeTypeId,
                        assignmentData.Contracts.ContractNumber,
                        assignmentData.Contracts.StartDate,
                        assignmentData.Contracts.EndDate,
                        assignmentData.Contracts.Salary,
                        assignmentData.Contracts.BucketName,
                        assignmentData.Contracts.FileName
                    );

                employee.AddAssignment
                    (
                        assignmentData.PositionId,
                        assignmentData.DepartmentId,
                        assignmentData.BranchId,
                        assignmentData.ManagerId,
                        assignmentData.HireDate,
                        assignmentData.TerminationDate,
                        assignmentData.StatusId,
                        contract.Id 
                    );
            }

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
