using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Repository
{
    public sealed class EducationLevelRepository(IDirectoryContext context) : BaseRepository<EducationLevel, IDirectoryContext>(context), IEducationLevelRepository
    {
        public async Task<EducationLevel?> GetByIdAsync(Guid id)
            => await _context.EducationLevels.FirstOrDefaultAsync(x => x.Id == id);
    }
}
