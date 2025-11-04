using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Repository
{
    public sealed class AdmissionStatusRepository(IDirectoryContext context) : BaseRepository<AdmissionStatus, IDirectoryContext>(context), IAdmissionStatusRepository
    {
        public async Task<AdmissionStatus?> GetByIdAsync(Guid id)
            => await _context.AdmissionStatuses.FirstOrDefaultAsync(x => x.Id == id);
    }
}
