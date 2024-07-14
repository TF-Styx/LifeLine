using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class DocumentPatient
{
    public int IdDocumentPatient { get; set; }

    public int IdTypeDocument { get; set; }

    public int IdPatient { get; set; }

    public string Number { get; set; }

    public string PlaceOfIssue { get; set; }

    public string DateOfIssue { get; set; }

    public byte[] DocumentImage { get; set; }

    public string DocumentFile { get; set; }
}
