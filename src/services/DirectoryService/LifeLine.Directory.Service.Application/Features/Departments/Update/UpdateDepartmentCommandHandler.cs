using MediatR;
using Terminex.Common.Results;
using Shared.Kernel.Exceptions;
using Shared.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Domain.ValueObjects;
using LifeLine.Directory.Service.Application.Common.Repository;

namespace LifeLine.Directory.Service.Application.Features.Departments.Update
{
    public sealed class UpdateDepartmentCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository,
            ILogger<UpdateDepartmentCommandHandler> logger
        ) : IRequestHandler<UpdateDepartmentCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly IDepartmentRepository _repository = repository;
        private readonly ILogger<UpdateDepartmentCommandHandler> _logger = logger;

        public async Task<Result> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _repository.GetByIdAsync(request.Id);

                if (department == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись департамента не найдена!"));

                department.UpdateName(DirectoryName.Create(request.Name));
                department.UpdateDescription(Description.Create(request.Description));

                department.UpdateAddress
                (
                    Address.Create
                    (
                        request.Address.PostalCode,
                        request.Address.Region,
                        request.Address.City,
                        request.Address.Street,
                        request.Address.Building
                    )
                );

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при обновлении Department!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
