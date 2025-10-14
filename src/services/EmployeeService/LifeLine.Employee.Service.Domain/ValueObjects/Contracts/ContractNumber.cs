using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;
using System.Text.RegularExpressions;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Contracts
{
    public sealed record ContractNumber
    {
        public string Value { get; }

        private ContractNumber(string value) => Value = value;
        
        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="IncorrectStringException"></exception>
        public static ContractNumber Create(string value)
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(value), () => new EmptyNameException("Номер контракта не может быть пустым!"));
            GuardException.Against.That(!Regex.IsMatch(value, @"^\d+-ТД\/\d{4}$"), () => new IncorrectStringException($"Неверный формат номера контракта! Пример: '125-ТД/2024'"));

            return new ContractNumber(value);
        }

        public override string ToString() => Value.ToString();

        public static implicit operator string(ContractNumber contractNumber) => contractNumber.Value;
    }
}
