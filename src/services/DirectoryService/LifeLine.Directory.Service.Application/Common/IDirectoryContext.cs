using LifeLine.Directory.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.Directory.Service.Application.Common
{
    public interface IDirectoryContext : IBaseWriteDbContext
    {
        DbSet<Hospital> Hospitals { get; set; }
        DbSet<Branch> Branches { get; set; }
        DbSet<Department> Departments { get; set; }
        DbSet<Position> Positions { get; set; }
        DbSet<Status> Statuses { get; set; }

        DbSet<AdmissionStatus> AdmissionStatuses { get; set; }
        DbSet<DocumentType> DocumentTypes { get; set; }
        DbSet<EducationLevel> EducationLevels { get; set; }
        DbSet<PermitType> PermitTypes { get; set; }
    }
}
