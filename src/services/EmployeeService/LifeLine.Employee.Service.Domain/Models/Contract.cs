using LifeLine.Employee.Service.Domain.ValueObjects.Contracts;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.Employee.Service.Domain.ValueObjects.EmployeeType;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;

namespace LifeLine.Employee.Service.Domain.Models
{
    public sealed class Contract : Aggregate<ContractId>
    {
        public EmployeeId EmployeeId { get; private set; }
        public EmployeeTypeId EmployeeTypeId { get; private set; }
        public ContractNumber ContractNumber { get; private set; } = null!;
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public Salary Salary { get; private set; }
        public ImageKey? FileKey { get; private set; }

        private Contract() { }
        private Contract
            (
                ContractId id, 
                EmployeeId employeeId, 
                EmployeeTypeId employeeTypeId, 
                ContractNumber contractNumber, 
                DateTime startDate, 
                DateTime endDate, 
                Salary salary, 
                ImageKey? fileKey
            ) : base(id)
        {
            EmployeeId = employeeId;
            EmployeeTypeId = employeeTypeId;
            ContractNumber = contractNumber;
            StartDate = startDate;
            EndDate = endDate;
            Salary = salary;
            FileKey = fileKey;
        }

        public static Contract Create
            (
                Guid employeeId, 
                Guid employeeTypeId, 
                string contractNumber, 
                DateTime startDate, 
                DateTime endDate, 
                decimal salary, 
                ImageKey? fileKey
            ) 
            => new Contract
                (
                    ContractId.New(), 
                    EmployeeId.Create(employeeId), 
                    EmployeeTypeId.Create(employeeTypeId), 
                    ContractNumber.Create(contractNumber), 
                    startDate, 
                    endDate, 
                    Salary.FromRubles(salary),
                    fileKey
                );
    }
}
