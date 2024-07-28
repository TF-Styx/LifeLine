using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class TypeDocument
{
    public int IdTypeDocument { get; set; }

    public string TypeDocumentName { get; set; }

    public int IdTypeOfPersone { get; set; }

    public virtual ICollection<DocumentPatient> DocumentPatients { get; set; } = new List<DocumentPatient>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual TypeOfPersone IdTypeOfPersoneNavigation { get; set; }
}
