using System;
using System.Collections.Generic;

namespace LifeLine.MVVM.Models.MSSQL_DB;

public partial class TypeOfPersone
{
    public int IdTypeOfPersone { get; set; }

    public string TypeOfPersoneName { get; set; }

    public virtual ICollection<TypeDocument> TypeDocuments { get; set; } = new List<TypeDocument>();
}
