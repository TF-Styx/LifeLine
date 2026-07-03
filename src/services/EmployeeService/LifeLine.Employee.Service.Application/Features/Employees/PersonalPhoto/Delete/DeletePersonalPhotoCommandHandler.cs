using LifeLine.Employee.Service.Application.Features.Employees.EducationDocument.Delete;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Terminex.Common.Primitives;
using Terminex.Common.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalPhoto.Delete
{
    public sealed class DeletePersonalPhotoCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<DeletePersonalPhotoCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(DeletePersonalPhotoCommand request, CancellationToken cancellationToken)
        {
            var employee = await repository.GetByIdAsync(request.EmployeeId);

            if (employee == null)
                return Error.NotFound("Пользователь не найден!");

            employee.DeletePersonalPhoto();

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
