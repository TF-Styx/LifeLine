using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.EmployeeService;

namespace LifeLine.Employee.Service.Application.Features.Employees.Get.GetAll
{
    public sealed class GetAllEmployeeQueryHandler(IReadContext context) : IRequestHandler<GetAllEmployeeQuery, List<EmployeeResponse>>
    {
        private readonly IReadContext _context = context;

        public async Task<List<EmployeeResponse>> Handle(GetAllEmployeeQuery request, CancellationToken cancellationToken)
            => await _context.EmployeeAdminListItemViews
                .Select(x => new EmployeeResponse(x.Id, x.Surname, x.Name, x.Patronymic, x.GenderId, x.GenderName))
                    .ToListAsync(cancellationToken);
    }
}
