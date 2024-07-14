using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class Patient
{
    public int IdPatient { get; set; }

    public int IdDepartment { get; set; }

    public int IdEmployee { get; set; }

    public string SecondName { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int? RoomNumber { get; set; }
}
