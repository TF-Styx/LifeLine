using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.EmployeeService.Application.Abstraction.Common.Abstraction;
using LifeLine.EmployeeService.Application.Abstraction.Common.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Domain.Exceptions;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Exceptions;
using Shared.Kernel.Results;

namespace LifeLine.Employee.Service.Application.Features.Employees.Create
{
    public sealed class CreateEmployeeCommandHandler
        (
            IWriteContext context,
            IEmployeeRepository repository,
            ILogger<CreateEmployeeCommandHandler> logger
        ) : IRequestHandler<CreateEmployeeCommand, Result>
    {
        public readonly IWriteContext _context = context;
        public readonly IEmployeeRepository _repository = repository;
        public readonly ILogger<CreateEmployeeCommandHandler> _logger = logger;

        public async Task<Result> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var employee = Domain.Models.Employee.Create(request.Surname, request.Name, request.Patronymic, request.GenderId);

                //List<PersonalDocuments>
                if (request.PersonalDocuments != null)
                    foreach (var item in request.PersonalDocuments) 
                        employee.AddPersonalDocument(item.DocumentTypeId, item.Number, item.Series, null);

                //ContactInformation
                if (request.CreateContactInfo != null)
                    employee.AddContactInformation
                        (
                            request.CreateContactInfo.PersonalPhone, 
                            request.CreateContactInfo.CorporatePhone, 
                            request.CreateContactInfo.PersonalEmail, 
                            request.CreateContactInfo.CorporateEmail,
                            Address.Create
                                (
                                    request.CreateContactInfo.Address.PostalCode,
                                    request.CreateContactInfo.Address.Region,
                                    request.CreateContactInfo.Address.City,
                                    request.CreateContactInfo.Address.Street,
                                    request.CreateContactInfo.Address.Building,
                                    request.CreateContactInfo.Address.Apartment
                                )
                        );

                //List<EducationDocuments>
                if (request.CreateEducationDoc != null)
                    foreach (var item in request.CreateEducationDoc)
                        employee.AddEducationDocument
                            (
                                item.EducationLevelId,
                                item.DocumentTypeId,
                                item.DocumentNumber,
                                item.IssuedDate.ToUniversalTime(),
                                item.OrganizationName,
                                item.QualificationAwardedName,
                                item.SpecialtyName,
                                item.ProgramName,
                                item.TotalHours
                            );

                //List<WorkPermit>
                if (request.CreateWorkPermit != null)
                    foreach (var item in request.CreateWorkPermit)
                        employee.AddWorkPermit
                            (
                                item.WorkPermitName, 
                                item.DocumentSeries, 
                                item.WorkPermitNumber, 
                                item.ProtocolNumber, 
                                item.SpecialtyName, 
                                item.IssuingAuthority, 
                                item.IssueDate, 
                                item.ExpiryDate, 
                                item.PermitTypeId, 
                                item.AdmissionStatusId
                            );

                await _repository.AddAsync(employee, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException domainEX)
            {
                if (domainEX is EmptyIdentifierException emptyEX)
                {
                    _logger.LogCritical(emptyEX, $"В методе '{nameof(Domain.Models.Employee.Create)}', в '{nameof(CreateEmployeeCommandHandler)}' при создании сотрудника не был сгенерирован {nameof(EmployeeId)}, в виде Guid!");
                    return Result.Failure(new Error(ErrorCode.Create, "Ошибка на стороне сервера!"));
                }

                return Result.Failure(new Error(ErrorCode.Create, domainEX.Message));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при создании Employee!");

                return Result.Failure(new Error(ErrorCode.Server, "Ошибка сервера при сохранении!"));
            }
        }
    }
}
