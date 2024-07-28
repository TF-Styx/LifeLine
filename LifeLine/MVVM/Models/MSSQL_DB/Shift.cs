using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class Shift
{
    public int IdShift { get; set; }

    public string ShiftName { get; set; }

    public virtual ICollection<TimeTable> TimeTables { get; set; } = new List<TimeTable>();
}
