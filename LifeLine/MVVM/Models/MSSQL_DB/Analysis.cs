﻿using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class Analysis
{
    public int IdAnalysis { get; set; }

    public int? IdPatient { get; set; }

    public string DateTime { get; set; }

    public string Result { get; set; }

    public string ResultFile { get; set; }

    public int? IdStatus { get; set; }
}
