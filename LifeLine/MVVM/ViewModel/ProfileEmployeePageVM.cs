using LifeLine.MVVM.Models.MSSQL_DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeLine.MVVM.ViewModel
{
    class ProfileEmployeePageVM : BaseViewModel
    {
        Employee userEmployee;

        private string _textTest;
        public string TextTest
        {
            get => _textTest;
            set
            {
                _textTest = value;
                OnPropertyChanged();
            }
        }

        public ProfileEmployeePageVM(object user)
        {
            userEmployee = (Employee)user;
            TextTest = userEmployee.SecondName + " " + userEmployee.FirstName + " " + userEmployee.LastName;
        }
    }
}
