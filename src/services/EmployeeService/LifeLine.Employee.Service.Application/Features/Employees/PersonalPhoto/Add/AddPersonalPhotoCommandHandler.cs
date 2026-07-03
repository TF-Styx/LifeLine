using MediatR;
using Shared.Kernel.Errors;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalPhoto.Add
{
    public sealed class AddPersonalPhotoCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<AddPersonalPhotoCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(AddPersonalPhotoCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            var fileUrlResult = FileUrl.Create(request.BucketName, request.FileName);

            if (fileUrlResult.IsFailure)
                return new Error(AppErrors.Upload, "Ошибка зазрузки изображения!");

            employee.AddPersonalPhoto(fileUrlResult.Value);

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
