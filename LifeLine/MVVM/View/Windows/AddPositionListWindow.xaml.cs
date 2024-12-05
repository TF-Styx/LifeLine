using System.Windows;
using System.Windows.Input;

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
