using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Get.GetAll
{
    public sealed class GetAllEmployeeTypeQueryHandler(IReadContext context) : IRequestHandler<GetAllEmployeeTypeQuery, List<EmployeeTypeResponse>>
    {
        private readonly IReadContext _context = context;

        public async Task<List<EmployeeTypeResponse>> Handle(GetAllEmployeeTypeQuery request, CancellationToken cancellationToken)
            => await _context.EmployeeTypeViews.Select(x => new EmployeeTypeResponse(x.Id, x.Name, x.Description)).ToListAsync(cancellationToken);
    }
}
