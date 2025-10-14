﻿using LifeLine.Employee.Service.Domain.ValueObjects.Assignments;
using LifeLine.Employee.Service.Domain.ValueObjects.Contracts;
using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using Shared.Domain.ValueObjects;
using Shared.Kernel.Primitives;
using Shared.Domain.Exceptions;

namespace LifeLine.Employee.Service.Domain.Models
{
    //Кадровые назначения
    //или история работы сотрудника
    public sealed class Assignment : Aggregate<AssignmentId>
    {
        public EmployeeId EmployeeId { get; private set; }
        public PositionId PositionId { get; private set; }
        public DepartmentId DepartmentId { get; private set; }
        public EmployeeId? ManagerId { get; private set; }
        public DateTime HireDate { get; private set; }
        public DateTime TerminationDate { get; private set; }
        public StatusId StatusId { get; private set; }
        public ContractId ContractId { get; private set; }

        private Assignment() { }
        private Assignment
            (
                AssignmentId id, 
                EmployeeId employeeId, 
                PositionId positionId, 
                DepartmentId departmentId, 
                EmployeeId? managerId, 
                DateTime hireDate, 
                DateTime terminationDate, 
                StatusId statusId, 
                ContractId contractId
            ) : base(id)
        {
            EmployeeId = employeeId;
            PositionId = positionId;
            DepartmentId = departmentId;
            ManagerId = managerId;
            HireDate = hireDate;
            TerminationDate = terminationDate;
            StatusId = statusId;
            ContractId = contractId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="positionId"></param>
        /// <param name="departmentId"></param>
        /// <param name="managerId"></param>
        /// <param name="hireDate"></param>
        /// <param name="terminationDate"></param>
        /// <param name="statusId"></param>
        /// <param name="contractId"></param>
        /// <exception cref="EmptyIdentifierException"></exception>
        /// <exception cref="EmptySurnameException"></exception>
        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="EmptyPatronymicException"></exception>
        /// <exception cref="LengthException"></exception>
        /// <exception cref="IncorrectStringException"></exception>
        /// <returns></returns>
        public static Assignment Create
            (
                Guid employeeId, 
                Guid positionId, 
                Guid departmentId, 
                Guid? managerId, 
                DateTime hireDate, 
                DateTime terminationDate, 
                Guid statusId, 
                Guid contractId
            ) => new Assignment
                (
                    AssignmentId.New(), 
                    EmployeeId.Create(employeeId), 
                    PositionId.Create(positionId), 
                    DepartmentId.Create(departmentId), 
                    managerId != null ? EmployeeId.Create(managerId.Value) : EmployeeId.Null, 
                    hireDate, 
                    terminationDate, 
                    StatusId.Create(statusId), 
                    ContractId.Create(contractId)
                );
    }
}
