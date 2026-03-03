using MediatR;
using Terminex.Common.Results;

namespace LifeLine.Directory.Service.Application.Features.EducationLevels.Create
{
    public sealed record CreateEducationLevelCommand(string EducationLevelName) : IRequest<Result>;
}
