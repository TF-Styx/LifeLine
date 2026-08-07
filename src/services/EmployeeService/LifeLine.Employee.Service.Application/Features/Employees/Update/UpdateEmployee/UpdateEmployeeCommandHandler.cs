using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;
using LifeLine.Employee.Service.Domain.ValueObjects.Genders;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;

namespace LifeLine.Employee.Service.Application.Features.Employees.Update.UpdateEmployee
{
    public sealed class UpdateEmployeeCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository
        ) : IRequestHandler<UpdateEmployeeCommand, Result<Nothing>>
    {
        public async Task<Result<Nothing>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null)
                return Error.NotFound("Запись не найдена!");

            entity.UpdateSurname(Surname.Create(request.Surname));
            entity.UpdateName(Name.Create(request.Name));

            if (!string.IsNullOrWhiteSpace(request.Patronymic))
                entity.UpdatePatronymic(Patronymic.Create(request.Patronymic!));

            entity.UpdateGenderId(GenderId.Create(request.GenderId));

            await context.SaveChangesAsync(cancellationToken);

            return Nothing.Value;
        }
    }
}
