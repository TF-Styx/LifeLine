namespace LifeLine.EmployeeService.Application.Abstraction.Models
{
    public sealed class EmployeeTypeView
    {
        public Guid Id {  get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
