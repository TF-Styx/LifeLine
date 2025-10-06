using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Response.EmployeeService;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Genders.Get.GetById
{
    public sealed class GetByIdGenderQueryHandler(IReadContext context) : IRequestHandler<GetByIdGenderQuery, Result<GenderResponse>>
    {
        private readonly IReadContext _context = context;

        public async Task<Result<GenderResponse>> Handle(GetByIdGenderQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.GenderViews.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
                return Result<GenderResponse>.Failure(new Error(ErrorCode.NotFound, "Запись не найдена!"));

            return Result<GenderResponse>.Success(new GenderResponse(entity.Id.ToString(), entity.Name));
        }
    }
}
