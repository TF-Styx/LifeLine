using MediatR;
using Terminex.Common.Results;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Application.Common;

namespace LifeLine.Directory.Service.Application.Features.Statuses.Create
{
    public sealed class CreateStatusCommandHandler(IDirectoryContext context) : IRequestHandler<CreateStatusCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateStatusCommand request, CancellationToken cancellationToken)
        {
            var status = Status.Create(request.Name, request.Description);

            await context.Statuses.AddAsync(status, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return status.Id.ToString();
        }
    }
}
