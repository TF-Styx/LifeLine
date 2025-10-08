using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared.Domain.ValueObjects
{
    public sealed record Address
    {
        public const int MAX_POSTAL_CODE_LENGTH = 6;
        public const int MAX_REGION_LENGTH = 100;
        public const int MAX_CITY_LENGTH = 100;
        public const int MAX_STREET_LENGTH = 200;
        public const int MAX_BUILDING_LENGTH = 50;
        public const int MAX_APARTMENT_LENGTH = 50;

        // Свойства для хранения составных частей адреса
        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostalCode { get; }
        public string Region { get; }
        public string City { get; }
        public string Street { get; }
        public string Building { get; }
        public string? Apartment { get; }

        // Конструктор делаем приватным, чтобы создание шло только через фабричный метод Create
        private Address(string postalCode, string region, string city, string street, string building, string? apartment)
        {
            PostalCode = postalCode;
            Region = region;
            City = city;
            Street = street;
            Building = building;
            Apartment = apartment;
        }

        /// <exception cref="AddressException"></exception>
        public static Address Create
            (
                string postalCode,
                string region,
                string city,
                string street,
                string building,
                string? apartment = null
            )
        {
            GuardException.Against.That(string.IsNullOrWhiteSpace(postalCode), () => new AddressException("Почтовый индекс не может быть пустым!"));
            GuardException.Against.That(!Regex.IsMatch(postalCode, @"^\d{6}$"), () => new AddressException("Неверный формат почтового индекса (требуется 6 цифр)!"));
            GuardException.Against.That(string.IsNullOrWhiteSpace(region), () => new AddressException("Регион не может быть пустым!"));
            GuardException.Against.That(string.IsNullOrWhiteSpace(city), () => new AddressException("Город/населенный пункт не может быть пустым!"));
            GuardException.Against.That(string.IsNullOrWhiteSpace(street), () => new AddressException("Улица не может быть пустой!"));
            GuardException.Against.That(string.IsNullOrWhiteSpace(building), () => new AddressException("Номер дома не может быть пустым!"));
            GuardException.Against.That(apartment != null && string.IsNullOrWhiteSpace(apartment), () => new AddressException("Номер квартиры/офиса не может состоять только из пробелов!"));

            return new Address(postalCode.Trim(), region.Trim(), city.Trim(), street.Trim(), building.Trim(), apartment?.Trim());
        }

        /// <summary>
        /// Возвращает полное строковое представление адреса в стандартном формате.
        /// </summary>
        public override string ToString()
        {
            var addressBuilder = new StringBuilder();
            addressBuilder.Append($"Почтовый индекс: {PostalCode}, {Region}, г. {City}, ул. {Street}, д. {Building}");

            if (!string.IsNullOrWhiteSpace(Apartment))
            {
                addressBuilder.Append($", кв. {Apartment}");
            }

            return addressBuilder.ToString();
        }

        public static implicit operator string(Address address) => address.ToString();
    }
}
