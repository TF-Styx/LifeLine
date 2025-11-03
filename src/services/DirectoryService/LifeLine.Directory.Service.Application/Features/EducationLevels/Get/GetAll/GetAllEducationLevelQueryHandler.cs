using LifeLine.Directory.Service.Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.DirectoryService;

namespace LifeLine.Directory.Service.Application.Features.EducationLevels.Get.GetAll
{
    public sealed class GetAllEducationLevelQueryHandler(IDirectoryContext context) : IRequestHandler<GetAllEducationLevelQuery, List<EducationLevelResponse>>
    {
        private readonly IDirectoryContext _context = context;

        public async Task<List<EducationLevelResponse>> Handle(GetAllEducationLevelQuery request, CancellationToken cancellationToken)
            => await _context.EducationLevels.Select(x => new EducationLevelResponse(x.Id.ToString(), x.EducationLevelName)).ToListAsync(cancellationToken);
    }
}
