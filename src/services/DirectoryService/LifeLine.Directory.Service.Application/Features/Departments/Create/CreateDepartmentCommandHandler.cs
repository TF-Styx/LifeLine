using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Create
{
    public sealed class CreateDepartmentCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository,
            ILogger<CreateDepartmentCommandHandler> logger
        ) : IRequestHandler<CreateDepartmentCommand, Result<Guid>>
    {
        private readonly IDirectoryContext _context = context;
        private readonly IDepartmentRepository _repository = repository;
        private readonly ILogger<CreateDepartmentCommandHandler> _logger = logger;

        public async Task<Result<Guid>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = Department.Create
                    (
                        request.Name, 
                        request.Description,
                        Address.Create
                        (
                            request.Address.PostalCode,
                            request.Address.Region,
                            request.Address.City,
                            request.Address.Street,
                            request.Address.Building,
                            request.Address.Apartment
                        )
                    );

                foreach (var item in request.Positions)
                    department.AddPositions(item.Name, item.Description);

                await _repository.AddAsync(department, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<Guid>.Success(department.Id);
            }
            catch (DomainException domainEX)
            {
                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В методе '{nameof(Department.Create)}', в '{nameof(CreateDepartmentCommandHandler)}' при создании отдела не был сгенерирован {nameof(StatusId)}, в виде Guid!");
                    return Result<Guid>.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result<Guid>.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании Department!");

                return Result<Guid>.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
