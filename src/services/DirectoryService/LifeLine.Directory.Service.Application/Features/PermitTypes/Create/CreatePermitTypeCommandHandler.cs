using MediatR;
using Terminex.Common.Results;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Application.Common;

namespace LifeLine.Directory.Service.Application.Features.PermitTypes.Create
{
    public sealed class CreatePermitTypeCommandHandler(IDirectoryContext context) : IRequestHandler<CreatePermitTypeCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreatePermitTypeCommand request, CancellationToken cancellationToken)
        {
            var permitType = PermitType.Create(request.PermitTypeName);

            await context.PermitTypes.AddAsync(permitType, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return permitType.Id.ToString();
        }
    }
}
