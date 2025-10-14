using LifeLine.Employee.Service.Domain.Models;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Assignments.Create
{
    public sealed class CreateAssignmentCommandHandler
        (
            IWriteContext context,
            IAssignmentRepository assignmentRepository,
            IContractRepository contractRepository,
            ILogger<CreateAssignmentCommandHandler> logger
        ) : IRequestHandler<CreateAssignmentCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IAssignmentRepository _assignmentRepository = assignmentRepository;
        private readonly IContractRepository _contractRepository = contractRepository;
        private readonly ILogger<CreateAssignmentCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var contract = Contract.Create
                    (
                        request.EmployeeId, 
                        request.Contract.EmployeeTypeId, 
                        request.Contract.ContractNumber, 
                        request.Contract.StartDate, 
                        request.Contract.EndDate, 
                        request.Contract.Salary, 
                        null
                    );

                var assignment = Assignment.Create
                    (
                        request.EmployeeId,
                        request.PositionId,
                        request.DepartmentId,
                        request.ManagerId,
                        request.HireDate,
                        request.TerminationDate,
                        request.StatusId,
                        contract.Id
                    );

                await _contractRepository.AddAsync(contract, cancellationToken);
                await _assignmentRepository.AddAsync(assignment, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В процессе вызова '{nameof(CreateAssignmentCommandHandler)}' при создании занятости {nameof(Assignment)} и/или контракт {nameof(Contract)}, не был сгенерирован Guid!");
                    return Result.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании занятости или контракта!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
