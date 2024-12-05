using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.MVVM.ViewModel;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogServices;
using System.Windows.Controls;

namespace LifeLine.MVVM.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileEmployeePage.xaml
    /// </summary>
    public partial class ProfileEmployeePage : Page
    {
        public ProfileEmployeePage()
        {
            InitializeComponent();
            #region MyRegion
            //IDialogService service = new DialogService();

            //Func<EmployeeManagementContext> contextFactory = () => new EmployeeManagementContext();

            //IDataBaseService dataBaseServices = new DataBaseService(contextFactory);

            //DataContext = new ProfileEmployeePageVM(user, service, dataBaseServices);
            #endregion

        }
    }
}
