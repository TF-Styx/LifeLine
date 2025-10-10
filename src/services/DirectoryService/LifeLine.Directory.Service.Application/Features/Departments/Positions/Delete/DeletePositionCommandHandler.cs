using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Results;

namespace LifeLine.Directory.Service.Application.Features.Departments.Positions.Delete
{
    public sealed class DeletePositionCommandHandler
        (
            IDirectoryContext context,
            IDepartmentRepository departmentRepository,
            ILogger<DeletePositionCommandHandler> logger
        ) : IRequestHandler<DeletePositionCommand, Result>
    {
        private readonly IDirectoryContext _context = context;
        private readonly IDepartmentRepository _departmentRepository = departmentRepository;
        private readonly ILogger<DeletePositionCommandHandler> _logger = logger;

        public async Task<Result> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(request.DepartmentId);

                if (department == null)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Отдел не найден!"));

                var positionToRemove = department.GetPositionToRemove(request.PositionId);

                _context.Positions.Remove(positionToRemove);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при удалении должности!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
