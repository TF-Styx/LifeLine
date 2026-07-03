using MediatR;
using Terminex.Common.Results;
using Terminex.Common.Primitives;

namespace LifeLine.Employee.Service.Application.Features.Employees.PersonalPhoto.Add
{
    public sealed record AddPersonalPhotoCommand(Guid EmployeeId, string BucketName, string FileName) : IRequest<Result<Nothing>>;
}
