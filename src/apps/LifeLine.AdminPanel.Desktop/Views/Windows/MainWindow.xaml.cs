using LifeLine.AdminPanel.Desktop.ViewModels.Windows;
using System.Windows;

namespace LifeLine.AdminPanel.Desktop.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowVM vm)
        {
            this.DataContext = vm;

            InitializeComponent();

            this.Width = 550;
            this.Height = 650;
            this.ResizeMode = ResizeMode.NoResize;

            vm.AuthController.ResizeWindow += () =>
            {
                this.Width = 800;
                this.Height = 450;
                this.ResizeMode = ResizeMode.CanResize;

                var screenWidth = SystemParameters.PrimaryScreenWidth;
                var screenHeight = SystemParameters.PrimaryScreenHeight;

                this.Left = (screenWidth / 2) - (this.ActualWidth / 2);
                this.Top = (screenHeight / 2) - (this.ActualHeight / 2);
            };
        }
    }
}
