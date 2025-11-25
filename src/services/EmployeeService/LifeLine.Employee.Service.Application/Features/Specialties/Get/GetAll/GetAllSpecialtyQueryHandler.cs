using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Specialties.Get.GetAll
{
    public sealed class GetAllSpecialtyQueryHandler(IWriteContext context) : IRequestHandler<GetAllSpecialtyQuery, List<SpecialtyResponse>>
    {
        private readonly IWriteContext _context = context;

        public async Task<List<SpecialtyResponse>> Handle(GetAllSpecialtyQuery request, CancellationToken cancellationToken)
            => await _context.Specialties.Select(x => new SpecialtyResponse(x.Id.ToString(), x.SpecialtyName, x.Description!)).ToListAsync(cancellationToken);
    }
}
