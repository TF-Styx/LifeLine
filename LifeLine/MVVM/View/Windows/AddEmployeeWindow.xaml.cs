using System.Windows;
using System.Windows.Input;

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
