using LifeLine.Employee.Service.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;
using System.Globalization;

namespace LifeLine.Employee.Service.Domain.ValueObjects.Contracts
{
    public readonly record struct Salary
    {
        private static readonly CultureInfo RussianCulture = new("ru-RU");
        private const string CurrencySymbol = "₽";

        public decimal Value { get; }

        public static Salary Zero => new(0);

        private Salary(decimal value)
        {
            Value = value;
        }

        /// <exception cref="SalaryOutOfRangeException"></exception>
        public static Salary FromRubles(decimal value)
        {
            GuardException.Against.That(value <= 0, () => new SalaryOutOfRangeException("Сумма зарплаты не может быть отрицательной или равной нулю!"));

            return new Salary(Math.Round(value, 2));
        }

        #region Операторы и сравнение

        public static Salary operator +(Salary a, Salary b)
            => new(a.Value + b.Value);

        /// <exception cref="InvalidSalaryOperationException"></exception>
        public static Salary operator -(Salary a, Salary b)
        {
            var resultValue = a.Value - b.Value;

            GuardException.Against.That(resultValue < 0, () => new InvalidSalaryOperationException("Результат вычитания не может быть отрицательной денежной суммой!"));

            return new Salary(resultValue);
        }

        /// <exception cref="InvalidSalaryOperationException"></exception>
        public static Salary operator *(Salary money, decimal multiplier)
        {
            GuardException.Against.That(multiplier < 0, () => new InvalidSalaryOperationException("Множитель не может быть отрицательным!"));

            return FromRubles(money.Value * multiplier);
        }

        /// <exception cref="InvalidSalaryOperationException"></exception>
        public static Salary operator /(Salary money, decimal divisor)
        {
            GuardException.Against.That(divisor <= 0, () => new InvalidSalaryOperationException("Делитель должен быть положительным числом!"));

            return FromRubles(money.Value / divisor);
        }

        public static bool operator >(Salary a, Salary b) => a.Value > b.Value;
        public static bool operator <(Salary a, Salary b) => a.Value < b.Value;
        public static bool operator >=(Salary a, Salary b) => a.Value >= b.Value;
        public static bool operator <=(Salary a, Salary b) => a.Value <= b.Value;

        #endregion

        /// <summary>
        /// Возвращает сумму в российской валюте
        /// Например: "120 500,75 ₽".
        /// </summary>
        public override string ToString() => $"{Value.ToString("N2", RussianCulture)} {CurrencySymbol}";

        public static implicit operator decimal(Salary money) => money.Value;
    }
}
