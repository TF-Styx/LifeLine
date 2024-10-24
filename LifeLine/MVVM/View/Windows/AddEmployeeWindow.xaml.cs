using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.MVVM.ViewModel;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LifeLine.MVVM.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {
        public AddEmployeeWindow()
        {
            InitializeComponent();

            IDialogService service = new DialogService();

            Func<EmployeeManagementContext> contextFactory = () => new EmployeeManagementContext();

            IDataBaseServices dataBaseServices = new DataBaseServices(contextFactory);

            DataContext = new AddEmployeeVM(service, dataBaseServices);
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            //App.Current.Shutdown();
            SystemCommands.CloseWindow(this);
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (MyPopup.IsOpen && !IsMouseOverTarget(MyPopup, e))
            {
                MyPopup.IsOpen = false;
            }
        }
        private bool IsMouseOverTarget(FrameworkElement target, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(target);
            return mousePosition.X >= 0 && mousePosition.X <= target.ActualWidth
                   && mousePosition.Y >= 0 && mousePosition.Y <= target.ActualHeight;
        }
    }
}
