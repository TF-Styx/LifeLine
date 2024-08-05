using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class Department
{
    public int IdDepartment { get; set; }

    public string DepartmentName { get; set; }

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ICollection<PositionList> PositionLists { get; set; } = new List<PositionList>();
}
