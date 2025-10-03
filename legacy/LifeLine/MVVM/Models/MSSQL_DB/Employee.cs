using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class Employee
{
    public int IdEmployee { get; set; }

    public string SecondName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int IdPosition { get; set; }

    public int IdGender { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public byte[] Avatar { get; set; }

    public decimal? Salary { get; set; }

    public DateTime? DateAddition { get; set; }

    public DateTime? DateTakingOffice { get; set; }

    public DateTime? DateBirth { get; set; }

    public string PhoneNumber { get; set; }

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual Gender IdGenderNavigation { get; set; }

    public virtual Position IdPositionNavigation { get; set; }

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ICollection<TimeTable> TimeTables { get; set; } = new List<TimeTable>();
}
