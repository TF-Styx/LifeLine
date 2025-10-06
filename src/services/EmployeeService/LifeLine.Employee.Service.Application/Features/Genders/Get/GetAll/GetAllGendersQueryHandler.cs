using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Genders.Get.GetAll
{
    public sealed class GetAllGendersQueryHandler(IReadContext context) : IRequestHandler<GetAllGendersQuery, List<GenderResponse>>
    {
        private readonly IReadContext _context = context;

        public async Task<List<GenderResponse>> Handle(GetAllGendersQuery request, CancellationToken cancellationToken) 
            => await _context.GenderViews.Select(g => new GenderResponse(g.Id.ToString(), g.Name)).ToListAsync(cancellationToken);
    }
}
