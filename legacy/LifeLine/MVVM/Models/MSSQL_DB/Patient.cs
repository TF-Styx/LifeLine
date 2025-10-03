using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class Patient
{
    public int IdPatient { get; set; }

    public int IdDepartment { get; set; }

    public int IdEmployee { get; set; }

    public int IdGender { get; set; }

    public string SecondName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int? RoomNumber { get; set; }

    public virtual ICollection<Analysis> Analyses { get; set; } = new List<Analysis>();

    public virtual ICollection<DocumentPatient> DocumentPatients { get; set; } = new List<DocumentPatient>();

    public virtual Department IdDepartmentNavigation { get; set; }

    public virtual Employee IdEmployeeNavigation { get; set; }

    public virtual Gender IdGenderNavigation { get; set; }
}
