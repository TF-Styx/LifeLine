using MediatR;
using Terminex.Common.Results;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Application.Common;

namespace LifeLine.Directory.Service.Application.Features.EducationLevels.Create
{
    public sealed class CreateEducationLevelCommandHandler(IDirectoryContext context) : IRequestHandler<CreateEducationLevelCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateEducationLevelCommand request, CancellationToken cancellationToken)
        {
            var educationLevel = EducationLevel.Create(request.EducationLevelName);

            await context.EducationLevels.AddAsync(educationLevel, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return educationLevel.Id.ToString();
        }
    }
}
