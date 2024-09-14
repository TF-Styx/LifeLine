using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class TimeTable
{
    public int IdTimeTable { get; set; }

    public int IdEmployee { get; set; }

    public DateTime? Date { get; set; }

    public string TimeStart { get; set; }

    public string TimeEnd { get; set; }

    public int? IdShift { get; set; }

    public string Notes { get; set; }

    public virtual Employee IdEmployeeNavigation { get; set; }

    public virtual Shift IdShiftNavigation { get; set; }
}
