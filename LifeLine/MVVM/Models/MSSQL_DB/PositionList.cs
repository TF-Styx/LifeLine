using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class PositionList
{
    public int IdPositionList { get; set; }

    public string PositionListName { get; set; }

    public int? IdDepartment { get; set; }

    public virtual Department IdDepartmentNavigation { get; set; }

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}
