using LifeLine.Employee.Service.Domain.ValueObjects.Genders;
using Shared.Kernel.Primitives;
using Shared.Domain.Exceptions;
using Shared.Kernel.Guard;
using Shared.Kernel.Guard.Extensions;

namespace LifeLine.Employee.Service.Domain.Models
{
    public sealed class Gender : Aggregate<GenderId>
    {
        public GenderName Name { get; private set; } = null!;

        private Gender() { }
        private Gender(GenderId id, GenderName name) : base(id) => Name = name;

        /// <exception cref="EmptyNameException"></exception>
        /// <exception cref="LengthException"></exception>
        /// <exception cref="IncorrectStringException"></exception>
        public static Gender Create(string genderName) => new(GenderId.New(), GenderName.Create(genderName));


        /// <exception cref="IdenticalValuesException"></exception>
        public void UpdateName(GenderName name)
        {
            GuardException.Against.That(name == Name, () => new IdenticalValuesException("Вы ввели идентичное название поля, которое хотите изменить!"));

            Name = name;
        }
    }
}
