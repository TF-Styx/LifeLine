using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Repository
{
    public sealed class PermitTypeRepository(IDirectoryContext context) : BaseRepository<PermitType, IDirectoryContext>(context), IPermitTypeRepository
    {
        public async Task<PermitType?> GetByIdAsync(Guid id)
            => await _context.PermitTypes.FirstOrDefaultAsync(x => x.Id == id);
    }
}
