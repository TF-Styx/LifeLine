using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class Document
{
    public int IdDocument { get; set; }

    public int? IdEmployee { get; set; }

    public int? IdTypeDocument { get; set; }

    public string Number { get; set; }

    public string PlaceOfIssue { get; set; }

    public DateOnly? DateOfIssue { get; set; }

    public byte[] DocumentImage { get; set; }

    public string DocumentFile { get; set; }

    public virtual Employee IdEmployeeNavigation { get; set; }

    public virtual TypeDocument IdTypeDocumentNavigation { get; set; }
}
