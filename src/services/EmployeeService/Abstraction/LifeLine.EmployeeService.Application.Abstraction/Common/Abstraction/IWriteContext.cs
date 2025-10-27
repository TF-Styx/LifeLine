using LifeLine.Employee.Service.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Api.Infrastructure;

namespace LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction
{
    public interface IWriteContext : IBaseWriteDbContext
    {
        DbSet<Employee.Service.Domain.Models.Employee> Employees { get; set; }
        DbSet<ContactInformation> ContactInformations { get; set; }
        DbSet<Gender> Genders { get; set; }

        DbSet<Assignment> Assignments { get; set; }
        DbSet<Contract> Contracts { get; set; }
        DbSet<EmployeeType> EmployeeTypes { get; set; }

        DbSet<WorkPermit> WorkPermits { get; set; }
        DbSet<EducationDocument> EducationDocuments { get; set; }

        DbSet<Specialty> Specialties { get; set; }
        DbSet<EmployeeSpecialty> EmployeeSpecialties { get; set; }

        DbSet<PersonalDocument> PersonalDocuments { get; set; }
    }
}
