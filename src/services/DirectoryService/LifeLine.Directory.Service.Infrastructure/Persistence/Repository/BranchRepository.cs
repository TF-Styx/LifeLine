using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Repository
{
    public sealed class BranchRepository(IDirectoryContext context) : BaseRepository<Branch, IDirectoryContext>(context), IBranchRepository
    {
        public async Task<Branch?> GetByIdAsync(Guid id)
            => await _context.Branches.FirstOrDefaultAsync(x => x.Id == id);
    }
}
