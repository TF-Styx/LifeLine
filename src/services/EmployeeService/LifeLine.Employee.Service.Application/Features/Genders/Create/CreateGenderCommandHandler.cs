using AutoMapper;
using LifeLine.Employee.Service.Domain.Models;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Contracts.Response.EmployeeService;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Genders.Create
{
    public sealed class CreateGenderCommandHandler
        (
            IWriteContext context, 
            IGenderRepository repository, 
            IMapper mapper,
            ILogger<CreateGenderCommandHandler> logger
        ) : IRequestHandler<CreateGenderCommand, Result>
    {
        private readonly IWriteContext _context = context;
        private readonly IGenderRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateGenderCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreateGenderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = Gender.Create(request.Name);

                await _repository.AddAsync(entity, cancellationToken);

                var dto = _mapper.Map<GenderResponse>(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                //if (domainEX is EmptyIdentifierException emptyEX)
                //    return Result.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));

                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании Gender!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
