using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.EducationLevels.Create
{
    public sealed class CreateEducationLevelCommandHandler
        (
            IDirectoryContext context,
            ILogger<CreateEducationLevelCommandHandler> logger
        ) : IRequestHandler<CreateEducationLevelCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly ILogger<CreateEducationLevelCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreateEducationLevelCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var educationLevel = EducationLevel.Create(request.EducationLevelName);

                await _context.EducationLevels.AddAsync(educationLevel, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В методе '{nameof(EducationLevel.Create)}', в '{nameof(CreateEducationLevelCommandHandler)}' при создании уровень образования не был сгенерирован {nameof(EducationLevelId)}, в виде Guid!");
                    return Result<Guid>.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result<Guid>.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании EducationLevel!");

                return Result<Guid>.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
