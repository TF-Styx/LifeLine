using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Repository
{
    public sealed class StatusRepository(IDirectoryContext context) : BaseRepository<Status, IDirectoryContext>(context), IStatusRepository
    {
        public async Task<Status?> GetByIdAsync(Guid id)
            => await _context.Statuses.FirstOrDefaultAsync(x => x.Id == id);
    }
}
