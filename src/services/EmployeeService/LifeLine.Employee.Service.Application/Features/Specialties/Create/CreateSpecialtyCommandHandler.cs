using LifeLine.Employee.Service.Domain.Models;
using LifeLine.Employee.Service.Domain.ValueObjects.Specialties;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Specialties.Create
{
    public sealed class CreateSpecialtyCommandHandler
        (
            IWriteContext context,
            ISpecialtyRepository specialtyRepository,
            ILogger<CreateSpecialtyCommandHandler> logger
        ) : IRequestHandler<CreateSpecialtyCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly ISpecialtyRepository _specialtyRepository = specialtyRepository;
        private readonly ILogger<CreateSpecialtyCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var specialty = Specialty.Create(request.SpecialtyName, request.Description);

                await _specialtyRepository.AddAsync(specialty, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В методе '{nameof(Specialty.Create)}', в '{nameof(CreateSpecialtyCommandHandler)}' при создании специальности не был сгенерирован {nameof(SpecialtyId)}, в виде Guid!");
                    return Result.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании Specialty!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
