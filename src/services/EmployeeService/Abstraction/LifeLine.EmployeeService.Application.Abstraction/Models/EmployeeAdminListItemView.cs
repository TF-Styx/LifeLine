namespace LifeLine.EmployeeService.Application.Abstraction.Models
{
    public sealed class EmployeeAdminListItemView
    {
        public Guid Id { get; set; }
        public string Surname { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Patronymic { get; set; } = null!;
        public DateTime DateEntry { get; set; }
        public bool Rating { get; set; }
        public string? Avatar { get; set; }

        public Guid GenderId { get; set; }
        public string GenderName { get; set; } = null!;
    }
}
