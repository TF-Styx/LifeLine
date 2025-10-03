using LifeLine.MVVM.Models.MSSQL_DB;
using LifeLine.MVVM.ViewModel;
using LifeLine.Services.DataBaseServices;
using LifeLine.Services.DialogServices;
using System.Windows;
using System.Windows.Input;

namespace LifeLine.MVVM.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddTypeDocumentWindow.xaml
    /// </summary>
    public partial class AddTypeDocumentWindow : Window
    {
        public AddTypeDocumentWindow()
        {
            InitializeComponent();
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
    }
}
