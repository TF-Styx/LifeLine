using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.EmployeeService;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.EmployeeTypes.Get.GetById
{
    public sealed class GetByIdEmployeeTypeQueryHandler(IReadContext context) : IRequestHandler<GetByIdEmployeeTypeQuery, Result<EmployeeTypeResponse>>
    {
        private readonly IReadContext _context = context;

        public async Task<Result<EmployeeTypeResponse>> Handle(GetByIdEmployeeTypeQuery request, CancellationToken cancellationToken)
        {
            var employeeType = await _context.EmployeeTypeViews.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (employeeType == null)
                return Result<EmployeeTypeResponse>.Failure(new Error(ErrorCode.NotFound, "Запись не найдена!"));

            return Result<EmployeeTypeResponse>.Success(new EmployeeTypeResponse(employeeType.Id, employeeType.Name, employeeType.Description));
        }
    }
}
