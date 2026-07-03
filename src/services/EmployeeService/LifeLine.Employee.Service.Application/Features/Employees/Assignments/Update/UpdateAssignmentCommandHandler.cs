using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.Assignments.Update
{
    public sealed class UpdateAssignmentCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<UpdateAssignmentCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            employee.UpdateAssignmentPositionId(request.Id, request.PositionId);
            employee.UpdateAssignmentDepartmentId(request.Id, request.DepartmentId);
            employee.UpdateAssignmentBranchId(request.Id, request.BranchId);
            employee.UpdateAssignmentManagerId(request.Id, request.ManagerId);
            employee.UpdateAssignmentHireDate(request.Id, request.HireDate);
            employee.UpdateAssignmentTerminationDate(request.Id, request.TerminationDate);
            employee.UpdateAssignmentStatusId(request.Id, request.StatusId);

            employee.UpdateAssignmentContractEmploymentTypeId(request.Id, request.Contract.EmployeeTypeId);
            employee.UpdateAssignmentContractContractNumber(request.Id, request.Contract.ContractNumber);
            employee.UpdateAssignmentContractStartDate(request.Id, request.Contract.StartDate);
            employee.UpdateAssignmentContractEndDate(request.Id, request.Contract.EndDate);
            employee.UpdateAssignmentContractSalary(request.Id, request.Contract.Salary);
            employee.UpdateAssignmentContractFileKey(request.Id, request.Contract.BucketName, request.Contract.FileName);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
