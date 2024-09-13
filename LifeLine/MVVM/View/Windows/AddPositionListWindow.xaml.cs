using LifeLine.MVVM.ViewModel;
using LifeLine.Services.DialogService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LifeLine.MVVM.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddPositionListWindow.xaml
    /// </summary>
    public partial class AddPositionListWindow : Window
    {
        public AddPositionListWindow()
        {
            InitializeComponent();

            IDialogService dialogService = new DialogService();

            DataContext = new AddPositionListVM(dialogService);

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
