using MediatR;
using Terminex.Common.Results;
using Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions;
using Shared.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Departments.Create
{
    public sealed class CreateDepartmentCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository,
            ILogger<CreateDepartmentCommandHandler> logger
        ) : IRequestHandler<CreateDepartmentCommand, Result<string>>
    {
        private readonly IDirectoryContext _context = context;
        private readonly IDepartmentRepository _repository = repository;
        private readonly ILogger<CreateDepartmentCommandHandler> _logger = logger;

        public async Task<Result<string>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = Department.Create(request.Name, request.Description, request.Building, request.BranchId);

                await _repository.AddAsync(department, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<string>.Success(department.Id.ToString());
            }
            catch (DomainException domainEX)
            {
                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В методе '{nameof(Department.Create)}', в '{nameof(CreateDepartmentCommandHandler)}' при создании отдела не был сгенерирован {nameof(StatusId)}, в виде Guid!");
                    return Result<string>.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result<string>.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании Department!");

                return Result<string>.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
