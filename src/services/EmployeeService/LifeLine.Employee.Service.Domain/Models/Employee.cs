using LifeLine.Employee.Service.Domain.ValueObjects.Employees;
using LifeLine.Employee.Service.Domain.ValueObjects.Genders;
using Shared.Kernel.Primitives;
using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;
using Shared.Domain.ValueObjects;
using LifeLine.Employee.Service.Domain.Exceptions;

namespace LifeLine.Employee.Service.Domain.Models
{
    public sealed class Employee : Aggregate<EmployeeId>
    {
        public Surname Surname { get; private set; } = null!;
        public Name Name { get; private set; } = null!;
        public Patronymic? Patronymic { get; private set; }
        public DateTime DateEntry { get; private set; }
        public Rating Rating { get; private set; }
        public ImageKey? Avatar { get; private set; }
        public GenderId GenderId { get; private set; }

        public Gender Gender { get; private set; } = null!;
        public ContactInformation? ContactInformation { get; private set; }

        private Employee() { }
        private Employee(EmployeeId id, Surname surname, Name name, Patronymic patronymic, GenderId genderId) : base(id)
        {
            Surname = surname;
            Name = name;
            Patronymic = patronymic;
            Rating = Rating.DefaultRating;
            Avatar = ImageKey.Empty;
            GenderId = genderId;
        }

        /// <summary>
        /// Создание НОВОГО объекта сотрудника
        /// </summary>
        /// <param name="surname"></param>
        /// <param name="name"></param>
        /// <param name="patronymic"></param>
        /// <param name="genderId"></param>
        /// <exception cref="EmptyIdentifierException"></exception>
        /// <exception cref="EmptySurnameException"></exception>
        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="EmptyPatronymicException"></exception>
        /// <exception cref="LengthException"></exception>
        /// <exception cref="IncorrectStringException"></exception>
        /// <returns cref="Employee">НОВЫЙ объект Employee</returns>
        public static Employee Create(string surname, string name, string patronymic, Guid genderId)
            => new(EmployeeId.New(), Surname.Create(surname), Name.Create(name), Patronymic.Create(patronymic), GenderId.Create(genderId));

        /// <summary>
        /// При входе меняет дату входа
        /// </summary>
        public void Sign() => DateEntry = DateTime.UtcNow;

        /// <summary>
        /// Обновление поля ФАМИЛИИ сотрудника
        /// </summary>
        /// <param name="surname"></param>
        /// <exception cref="IdenticalValuesException"></exception>
        public void UpdateSurname(Surname surname)
        {
            GuardException.Against.That(surname == Surname, () => new IdenticalValuesException("Вы пытаетесь установить тоже значение!"));

            Surname = surname;
        }

        /// <summary>
        /// Обновление поля ИМЕНИ сотрудника
        /// </summary>
        /// <param name="name"></param>
        /// <exception cref="IdenticalValuesException"></exception>
        public void UpdateName(Name name)
        {
            GuardException.Against.That(name == Name, () => new IdenticalValuesException("Вы пытаетесь установить тоже значение!"));

            Name = name;
        }

        /// <summary>
        /// Обновление поля ОТЧЕСТВА сотрудника
        /// </summary>
        /// <param name="patronymic"></param>
        /// <exception cref="IdenticalValuesException"></exception>
        public void UpdatePatronymic(Patronymic patronymic)
        {
            GuardException.Against.That(patronymic == Patronymic, () => new IdenticalValuesException("Вы пытаетесь установить тоже значение!"));

            Patronymic = patronymic;
        }

        /// <summary>
        /// Обновление поля ГЕНДЕРА сотрудника
        /// </summary>
        /// <param name="genderId"></param>
        /// <exception cref="IdenticalValuesException"></exception>
        public void UpdateGenderId(GenderId genderId)
        {
            GuardException.Against.That(genderId == GenderId, () => new IdenticalValuesException("Вы пытаетесь установить тоже значение!"));

            GenderId = genderId;
        }

        #region ContactInformation
        /// <exception cref="ExistContactInformationException"></exception>
        /// <exception cref="EmptyIdentifierException"></exception>
        /// <exception cref="PhoneNumberException"></exception>
        /// <exception cref="EmailAddressException"></exception>
        public void AddContactInformation(string personalPhone, string? corporatePhone, string personalEmail, string? corporateEmail, Address address)
        {
            GuardException.Against.That(this.ContactInformation != null, () => new ExistContactInformationException("Контактная информация уже указана! Вы можете её изменить!"));

            var info = ContactInformation.Create(this.Id, personalPhone, corporatePhone, personalEmail, corporateEmail, address);

            this.ContactInformation = info;
        }

        /// <exception cref="EmptyContactInformationException"></exception>
        public void UpdatePersonalPhone(string personalPhone)
        {
            GuardException.Against.That(this.ContactInformation == null, () => new EmptyContactInformationException($"Контактной информации у пользователя: '{Surname} {Name} {Patronymic}' не существует!"));

            this.ContactInformation!.UpdatePersonalPhone(Phone.Create(personalPhone));
        }

        /// <exception cref="EmptyContactInformationException"></exception>
        public void UpdateCorporatePhone(string? corporatePhone)
        {
            GuardException.Against.That(this.ContactInformation == null, () => new EmptyContactInformationException($"Контактной информации у пользователя: '{Surname} {Name} {Patronymic}' не существует!"));

            this.ContactInformation!.UpdateCorporatePhone(corporatePhone != null ? Phone.Create(corporatePhone) : Phone.Null);
        }

        /// <exception cref="EmptyContactInformationException"></exception>
        public void UpdatePersonalEmail(string personalEmail)
        {
            GuardException.Against.That(this.ContactInformation == null, () => new EmptyContactInformationException($"Контактной информации у пользователя: '{Surname} {Name} {Patronymic}' не существует!"));

            this.ContactInformation!.UpdatePersonalEmail(Email.Create(personalEmail));
        }

        /// <exception cref="EmptyContactInformationException"></exception>
        public void UpdateCorporateEmail(string? corporateEmail)
        {
            GuardException.Against.That(this.ContactInformation == null, () => new EmptyContactInformationException($"Контактной информации у пользователя: '{Surname} {Name} {Patronymic}' не существует!"));

            this.ContactInformation!.UpdateCorporateEmail(corporateEmail != null ? Email.Create(corporateEmail) : Email.Null);
        }

        public void UpdateAddress(Address address)
        {
            GuardException.Against.That(this.ContactInformation == null, () => new EmptyContactInformationException($"Контактной информации у пользователя: '{Surname} {Name} {Patronymic}' не существует!"));

            this.ContactInformation!.UpdateAddress(address);
        }
        #endregion
    }
}
