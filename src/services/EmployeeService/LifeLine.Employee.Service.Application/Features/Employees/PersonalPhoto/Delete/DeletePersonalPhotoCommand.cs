using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalPhoto.Delete
{
    public sealed record DeletePersonalPhotoCommand(Guid EmployeeId) : IRequest<Result<Nothing>>;
}
