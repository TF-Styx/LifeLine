using MediatR;
using Terminex.Common.Results;
using LifeLine.Directory.Service.Domain.Models;
using LifeLine.Directory.Service.Application.Common;

namespace LifeLine.Directory.Service.Application.Features.AdmissionStatuses.Create
{
    public sealed class CreateAdmissionStatusCommandHandler(IDirectoryContext context) : IRequestHandler<CreateAdmissionStatusCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateAdmissionStatusCommand request, CancellationToken cancellationToken)
        {
            var admissionStatus = AdmissionStatus.Create(request.AdmissionName);

            await context.AdmissionStatuses.AddAsync(admissionStatus, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return admissionStatus.Id.ToString();
        }
    }
}
