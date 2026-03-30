using Shared.Kernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LifeLine.Employee.Service.Domain.Exceptions
{
    public sealed class ExistEmployeeSpecialtyExeption(string message) : DomainException(message);
}
