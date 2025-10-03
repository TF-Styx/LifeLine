using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class Gender
{
    public int IdGender { get; set; }

    public string GenderName { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();
}
