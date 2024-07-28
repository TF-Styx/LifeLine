using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class Position
{
    public int IdPosition { get; set; }

    public int IdPositionList { get; set; }

    public int IdDepartament { get; set; }

    public int? IdAccessLevel { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual AccessLevel IdAccessLevelNavigation { get; set; }

    public virtual Department IdDepartamentNavigation { get; set; }

    public virtual PositionList IdPositionListNavigation { get; set; }
}
