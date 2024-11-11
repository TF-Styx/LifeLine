using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeLine.MVVM.Models.AppModel
{
    public class EmployeeTimeTable
    {
        public int IdEmployee { get; set; }
        public byte[] Avatar { get; set; }
        public string SecondName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ObservableCollection<TimeTableList> TimeTableLists { get; set; }
        public ObservableCollection<ShiftIdName> ShiftIdNamesLists { get; set; }
    }

    public class TimeTableList
    {
        public int IdTimeTable { get; set; }
        public DateTime? Date { get; set; }
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
        public string ShiftNames { get; set; }
        public string Notes { get; set; }
    }

    public class ShiftIdName
    {
        public int IdShift { get; set; }
        public string ShiftName { get; set; }
    }
}
