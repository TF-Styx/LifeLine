using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class AccessLevel
{
    public int IdAccessLevel { get; set; }

    public string AccessLevelName { get; set; }

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}
