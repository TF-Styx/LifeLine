using LifeLine.Directory.Service.Application.Common;
using LifeLine.Directory.Service.Application.Common.Repository;
using LifeLine.Directory.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Infrastructure.Persistence.Repository
{
    public sealed class HospitalRepository(IDirectoryContext context) : BaseRepository<Hospital, IDirectoryContext>(context), IHospitalRepository
    {
        public async Task<Hospital?> GetByIdAsync(Guid id)
            => await _context.Hospitals.FirstOrDefaultAsync(x => x.Id == id);
    }
}
