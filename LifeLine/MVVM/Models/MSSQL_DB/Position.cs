using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class Position
{
    public int IdPosition { get; set; }

    public int IdPositionList { get; set; }

    public int IdDepartament { get; set; }

    public int IdAccessLevel { get; set; }
}
