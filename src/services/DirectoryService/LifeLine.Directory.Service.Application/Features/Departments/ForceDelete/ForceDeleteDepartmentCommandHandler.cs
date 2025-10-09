using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.ForceDelete
{
    public sealed class ForceDeleteDepartmentCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository repository,
            ILogger<ForceDeleteDepartmentCommandHandler> logger
        ) : IRequestHandler<ForceDeleteDepartmentCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly IDepartmentRepository _repository = repository;
        private readonly ILogger<ForceDeleteDepartmentCommandHandler> _logger = logger;

        public async Task<Result> Handle(ForceDeleteDepartmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _repository.GetByIdAsync(request.Id);

                if (department == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Запись департамента не найдена!"));

                _context.Positions.RemoveRange(department.Positions);
                
                _repository.Remove(department);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при удалении Department!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
